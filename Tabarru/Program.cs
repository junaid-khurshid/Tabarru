using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;
using Tabarru.Attributes;
using Tabarru.Repositories.DatabaseContext;
using Tabarru.Repositories.Implementation;
using Tabarru.Repositories.IRepository;
using Tabarru.Repositories.Models;
using Tabarru.RequestModels;
using Tabarru.Services.Implementation;
using Tabarru.Services.IServices;

namespace Tabarru
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            builder.Services.AddDbContext<DbStorageContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Services
            builder.Services.AddTransient<ICharityAccountService, CharityAccountService>();
            builder.Services.AddTransient<IEmailMessageService, EmailMessageService>();
            builder.Services.AddScoped<ICampaignService, CampaignService>();
            builder.Services.AddScoped<IPackageService, PackageService>();
            builder.Services.AddScoped<ITemplateService, TemplateService>();
            builder.Services.AddScoped<IDeviceService, DeviceService>();
            builder.Services.AddScoped<ICharityKycService, CharityKycService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();
            builder.Services.AddScoped<IFileStoringService, FileStoringService>();
            builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();

            // Repository
            builder.Services.AddTransient<ICharityRepository, CharityRepository>();
            builder.Services.AddTransient<IEmailVerificationRepository, EmailVerificationRepository>();
            builder.Services.AddScoped<ICampaignRepository, CampaignRepository>();
            builder.Services.AddScoped<IPackageRepository, PackageRepository>();
            builder.Services.AddScoped<ITemplateRepository, TemplateRepository>();
            builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();
            builder.Services.AddScoped<ICharityKycRepository, CharityKycRepository>();
            builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
            builder.Services.AddScoped<IGiftAidRepository, GiftAidRepository>();
            builder.Services.AddScoped<IRecurringPaymentRepository, RecurringPaymentRepository>();

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSingleton<IAuthorizationHandler, ValidateKycStatusPolicy>();

            // AWS S3
            builder.Services.Configure<AwsSettings>(
                builder.Configuration.GetSection("AWS")
            );

            builder.Services.AddSingleton<IAmazonS3>(sp =>
            {
                var aws = sp.GetRequiredService<IOptions<AwsSettings>>().Value;

                var credentials = new BasicAWSCredentials(
                    aws.AccessKey,
                    aws.SecretKey
                );

                var config = new AmazonS3Config
                {
                    RegionEndpoint = RegionEndpoint.GetBySystemName(aws.Region)
                };

                return new AmazonS3Client(credentials, config);
            });

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!);
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
                });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("KycApprovedOnly", policy =>
                    policy.Requirements.Add(new ValidateKycStatusPolicy()));
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Allow Swagger to call API (CORS fix)
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            var app = builder.Build();

            await InitializeDatabaseAsync(app.Services, app.Environment, app);

            // CORS must come before authentication
            app.UseCors("AllowAll");

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Comment out HTTPS redirect in Docker (causes "Failed to fetch")
            // app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.MapGet("/", () => "API is running...");

            app.Run();
        }

        private static async Task InitializeDatabaseAsync(IServiceProvider serviceProvider, IHostEnvironment env, WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<DbStorageContext>();
            Console.WriteLine("In the InitiazlizeDB Scope");

            while (true)
            {
                try
                {
                    Console.WriteLine("[DB Init] Database connection started");
                    if (await db.Database.CanConnectAsync())
                    {
                        Console.WriteLine("[DB Init] Database connection successful");

                        // Apply migrations — this creates the __EFMigrationsHistory table
                        await db.Database.MigrateAsync();
                        Console.WriteLine("[DB Init] Migrations applied ✅");
                    }
                    else
                    {
                        Console.WriteLine("[DB Init] Database not found — creating...");
                        // Only create database without skipping migrations
                        await db.Database.MigrateAsync();
                        Console.WriteLine("[DB Init] Database created and migrations applied ✅");
                    }

                    // Insert default PackageDetails if empty
                    Console.WriteLine("Checking DB for Package Details");
                    if (!db.Set<PackageDetails>().Any())
                    {
                        Console.WriteLine("Inserting Data in Package Details");
                        var filePath = Path.Combine(env.ContentRootPath, "Defaults", "PackageDetails.json");
                        if (File.Exists(filePath))
                        {
                            var packageRepository = scope.ServiceProvider.GetRequiredService<IPackageRepository>();
                            var packageDetailsList = GetDefaultValues<List<PackageDetails>>(filePath);

                            foreach (var package in packageDetailsList!)
                            {
                                await packageRepository.AddAsync(package);
                            }
                        }
                    }

                    break;
                }
                catch (SqlException ex)
                {
                    Console.WriteLine($"[DB Init] Waiting for SQL Server... Error: {ex.Message}");
                    await Task.Delay(5000);
                }
            }
        }

        private static T GetDefaultValues<T>(string filePath)
        {
            var json = File.ReadAllText(filePath);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return System.Text.Json.JsonSerializer.Deserialize<T>(json, options)!;
        }
    }
}