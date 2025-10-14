using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net;
using Tabarru.Common.Enums;
using Tabarru.Common.Helper;
using Tabarru.Common.Models;
using Tabarru.Repositories.DatabaseContext;
using Tabarru.Repositories.IRepository;
using Tabarru.Repositories.Models;
using Tabarru.Services.IServices;
using Tabarru.Services.Models;

namespace Tabarru.Services.Implementation
{
    public class CharityAccountService : ICharityAccountService
    {
        private readonly ICharityRepository charityRepository;
        private readonly IEmailVerificationRepository emailVerificationRepository;
        private readonly IPackageRepository packageRepository;
        private readonly IEmailMessageService emailMessageService;
        private readonly DbContext dbContext;
        private readonly IConfiguration configuration;

        public CharityAccountService(ICharityRepository charityRepository,
            IEmailVerificationRepository emailVerificationRepository,
            IPackageRepository packageRepository,
            IEmailMessageService emailMessageService,
            DbStorageContext dbContext,
            IConfiguration configuration)
        {
            this.charityRepository = charityRepository;
            this.emailVerificationRepository = emailVerificationRepository;
            this.packageRepository = packageRepository;
            this.emailMessageService = emailMessageService;
            this.dbContext = dbContext;
            this.configuration = configuration;
        }

        public async Task<CharityDetailDto> GetByEmail(string email)
        {
            var charity = await this.charityRepository.GetByEmailAsync(email);

            return new CharityDetailDto
            {
                Email = email,
                KycStatus = charity.KycStatus,
                Role = charity.Role,
            };
        }

        public async Task<Response<LoginResponse>> Login(LoginDto dto)
        {
            var charity = await this.charityRepository.GetByEmailAsync(dto.Email);
            if (charity == null)
                return new Response<LoginResponse>(HttpStatusCode.NotFound, new LoginResponse { EmailConfirmed = false }, "Sorry! Charity does not exists with this email address..", ResponseCode.Errors);

            if (charity.EmailVerified == false)
                return new Response<LoginResponse>(HttpStatusCode.BadRequest, new LoginResponse { EmailConfirmed = false }, "Sorry! Email is not verified.", ResponseCode.Errors);


            if (!GenerateHashAndSaltHelper.IsValidStringHash(dto.Password, charity.PasswordHash, charity.Salt))
            {
                return new Response<LoginResponse>(HttpStatusCode.Unauthorized, new LoginResponse { EmailConfirmed = false }, "Invalid email or password. Please try again.", ResponseCode.Errors);
            }

            var token = GenerateTokenHelper.CreateToken(this.configuration, charity.Id, charity.Email, charity.Role);
            var refreshToken = GenerateTokenHelper.GenerateRefreshToken();

            charity.RefreshToken = refreshToken.Item1;
            charity.RefreshTokenExpiryTime = refreshToken.Item2;

            var res = await this.charityRepository.UpdateAsync(charity);

            if (!res)
            {
                return new Response<LoginResponse>(HttpStatusCode.Unauthorized, new LoginResponse { EmailConfirmed = false }, "Invalid email or password. Please try again.", ResponseCode.Errors);
            }

            return new Response<LoginResponse>(HttpStatusCode.OK, new LoginResponse
            {
                EmailConfirmed = true,
                AccessToken = token.Item1,
                RefreshToken = refreshToken.Item1,
                ExpiresIn = token.Item2
            },
                "Charity Login Successfully.", ResponseCode.Data);
        }

        public async Task<Response> Register(CharityDetailDto dto)
        {

            using (var transaction = await dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var alreadyAddedCharity = await this.charityRepository.GetByEmailAsync(dto.Email);
                    if (alreadyAddedCharity != null)
                    {
                        await transaction.RollbackAsync();
                        return new Response(HttpStatusCode.BadRequest, $"Email {dto.Email} is already taken.");
                    }

                    (byte[], byte[]) hashSalt = GenerateHashAndSaltHelper.CreatePasswordHash(dto.Password);

                    var charity = new Charity
                    {
                        Email = dto.Email,
                        KycStatus = CharityKycStatus.Pending,
                        PasswordHash = hashSalt.Item2,
                        Salt = hashSalt.Item1,
                        Role = dto.Role.ToUpper(),
                        EmailVerified = false,
                        PackageId = "0"
                    };

                    var addRegisterResult = await this.charityRepository.AddAsync(charity);

                    var verificationDetails = await this.emailVerificationRepository.GetByEmailAsync(dto.Email);
                    if (verificationDetails != null)
                    {
                        await transaction.RollbackAsync();
                        return new Response(HttpStatusCode.BadRequest, $"Email {dto.Email} is already taken.");
                    }

                    charity = await this.charityRepository.GetByEmailAsync(dto.Email);
                    var emailVerificationCode = GenerateRandomNumberTokenHelper.GenerateRandomNumberToken(6);

                    var emailVerification = new EmailVerificationDetails
                    {
                        Email = dto.Email,
                        Token = emailVerificationCode,
                        ExpiryTime = DateTime.UtcNow.AddMinutes(30),
                        CharityId = charity?.Id,
                        IsUsed = false
                    };

                    string finalHtml = GetEmailTemplate(emailVerificationCode);

                    //Send Email Work
                    var success = await this.emailMessageService.SendEmailAsync(
                    dto.Email,
                    "Email Verification",
                    finalHtml
                    );

                    var addEmailVerifyDetails = await this.emailVerificationRepository.AddAsync(emailVerification);
                    await transaction.CommitAsync();

                    if (!addRegisterResult || !addEmailVerifyDetails)
                    {
                        await transaction.RollbackAsync();
                        return new Response(HttpStatusCode.BadRequest, "Charity Registration Failed");
                    }

                    return new Response(HttpStatusCode.Created, "Charity Registered Successfully");
                }

                catch (Exception ex)
                {
                    // Rollback if any error occurs
                    await transaction.RollbackAsync();
                    throw ex;
                }
            }
        }

        private static string GetEmailTemplate(string emailVerificationCode)
        {
            Console.WriteLine($" directory : {Directory.GetCurrentDirectory()}");

            string htmlPath1 = Path.Combine(Directory.GetCurrentDirectory(), "Tabarru.Common", "HtmlTemplates", "EmailVerificationBody.html");
            string htmlPath = Path.Combine(AppContext.BaseDirectory, "HtmlTemplates", "EmailVerificationBody.html");

            Console.WriteLine($" html path 1 : {htmlPath1}");
            Console.WriteLine($" html path : {htmlPath}");

            if (!File.Exists(htmlPath))
                throw new FileNotFoundException($"Email template not found at {htmlPath}");

            string htmlContent = File.ReadAllText(htmlPath);

            string finalHtml = htmlContent.Replace("{{CODE}}", emailVerificationCode);
            return finalHtml;
        }

        public async Task<Response> ReGenerateEmailVerificationTokenByEmail(string email)
        {
            var verificationDetails = await this.emailVerificationRepository.GetByEmailAsync(email);

            var emailVerificationCode = GenerateRandomNumberTokenHelper.GenerateRandomNumberToken(6);

            var verification = new EmailVerificationDetails
            {
                Email = email,
                Token = emailVerificationCode,
                ExpiryTime = DateTime.UtcNow.AddMinutes(30)
            };

            var res = await this.emailVerificationRepository.AddAsync(verification);

            if (!res)
            {
                return new Response(HttpStatusCode.BadRequest, "Confirmation email failed.");
            }

            return new Response(HttpStatusCode.Created, "Confirmation email sent successfully.");
        }

        public async Task<Response> VerifyToken(VerifyRequestDto request)
        {
            var verificationDetails = await this.emailVerificationRepository.GetByEmailAndIsNotUsedAsync(request.Email);

            if (verificationDetails == null || verificationDetails.ExpiryTime < DateTime.UtcNow)
                return new Response(HttpStatusCode.BadRequest, "Invalid or expired token.");

            verificationDetails.IsUsed = true;

            var res = await this.emailVerificationRepository.UpdateAsync(verificationDetails);

            var charity = await this.charityRepository.GetByEmailAsync(request.Email);
            if (charity == null)
            {
                return new Response(HttpStatusCode.BadRequest, "Email verification failed.");
            }

            charity.EmailVerified = true;
            var updateCharity = await this.charityRepository.UpdateAsync(charity);

            if (!res || !updateCharity)
            {
                return new Response(HttpStatusCode.BadRequest, "Email verification failed.");
            }

            return new Response(HttpStatusCode.Accepted, "Email verified successfully.");
        }

        public async Task<Response> AssignPackageAsync(CharityPackageUpdateDto dto)
        {
            var charity = await charityRepository.GetByIdAsync(dto.CharityId);
            if (charity == null)
                return new Response(HttpStatusCode.NotFound, "Charity not found");

            if (!charity.IsKycVerified)
                return new Response(HttpStatusCode.BadRequest, "KYC not verified. Cannot assign package.");

            var package = await packageRepository.GetPackageByIdAsync(dto.PackageId);
            if (package == null)
                return new Response(HttpStatusCode.NotFound, "Package not found");

            charity.PackageId = package.Id.ToString();
            charity.IsPackageVerified = true;

            await charityRepository.UpdateAsync(charity);

            return new Response(HttpStatusCode.OK, $"Package '{package.Name}' assigned successfully");
        }

        public async Task<Response<CharityReadDto>> GetCharityDetailsAsync(string ChairtyId)
        {
            var charities = await charityRepository.GetCharityAllDetailsByIdAsync(ChairtyId);

            return new Response<CharityReadDto>(HttpStatusCode.OK, charities.MapToDto(), ResponseCode.Data);
        }

        //public async Task<Response<LoginResponse>> GenerateRefreshToken(string token)
        //{

        //}
    }
}
