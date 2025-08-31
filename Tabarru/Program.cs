
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;
using Tabarru.Attributes;
using Tabarru.Repositories.DatabaseContext;
using Tabarru.Repositories.Implementation;
using Tabarru.Repositories.IRepository;
using Tabarru.Repositories.Models;
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
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddDbContext<DbStorageContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            //Services
            builder.Services.AddTransient<ICharityAccountService, CharityAccountService>();
            builder.Services.AddTransient<IEmailMessageService, EmailMessageService>();
            builder.Services.AddScoped<ICampaignService, CampaignService>();
            builder.Services.AddScoped<IPackageService, PackageService>();
            builder.Services.AddScoped<ITemplateService, TemplateService>();
            builder.Services.AddScoped<IDeviceService, DeviceService>();
            builder.Services.AddScoped<ICharityKycService, CharityKycService>();

            //Repository
            builder.Services.AddTransient<ICharityRepository, CharityRepository>();
            builder.Services.AddTransient<IEmailVerificationRepository, EmailVerificationRepository>();
            builder.Services.AddScoped<ICampaignRepository, CampaignRepository>();
            builder.Services.AddScoped<IPackageRepository, PackageRepository>();
            builder.Services.AddScoped<ITemplateRepository, TemplateRepository>();
            builder.Services.AddScoped<ITemplateCampaignRepository, TemplateCampaignRepository>();
            builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();
            builder.Services.AddScoped<ICharityKycRepository, CharityKycRepository>();

            // Register IHttpContextAccessor(needed in your policy)
            builder.Services.AddHttpContextAccessor();

            // Register custom authorization handler
            builder.Services.AddSingleton<IAuthorizationHandler, ValidateKycStatusPolicy>();

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

            // Add Swagger services
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            await InitializeDatabaseAsync(app.Services, app.Environment);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        private static async Task InitializeDatabaseAsync(IServiceProvider serviceProvider, IHostEnvironment env)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DbStorageContext>();

            // Check if any PackageDetails exist
            if (!context.Set<PackageDetails>().Any())
            {
                var packageRepository = scope.ServiceProvider.GetRequiredService<IPackageRepository>();

                var packageDetailsList = GetDefaultValues<List<PackageDetails>>(
                    Path.Combine(env.ContentRootPath, "Defaults", "PackageDetails.json")
                );

                foreach (var package in packageDetailsList)
                {
                    await packageRepository.AddAsync(package);
                }
            }
        }

        private static T GetDefaultValues<T>(string filePath)
        {
            var json = File.ReadAllText(filePath);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // 👈 allows "features" → Features
            };

            return System.Text.Json.JsonSerializer.Deserialize<T>(json, options)!;
        }

    }
}
