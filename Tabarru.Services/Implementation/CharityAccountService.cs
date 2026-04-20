using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net;
using Tabarru.Common.Enums;
using Tabarru.Common.Helper;
using Tabarru.Common.Models;
using Tabarru.Repositories.DatabaseContext;
using Tabarru.Repositories.Implementation;
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
        private readonly ICharityKycRepository charityKycRepository;
        private readonly IPasswordResetRepository passwordResetRepository;

        public CharityAccountService(ICharityRepository charityRepository,
            IEmailVerificationRepository emailVerificationRepository,
            IPackageRepository packageRepository,
            IEmailMessageService emailMessageService,
            DbStorageContext dbContext,
            IConfiguration configuration,
            ICharityKycRepository charityKycRepository,
            IPasswordResetRepository passwordResetRepository)
        {
            this.charityRepository = charityRepository;
            this.emailVerificationRepository = emailVerificationRepository;
            this.packageRepository = packageRepository;
            this.emailMessageService = emailMessageService;
            this.dbContext = dbContext;
            this.configuration = configuration;
            this.charityKycRepository = charityKycRepository;
            this.passwordResetRepository = passwordResetRepository;
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

        public async Task<Response> Register(CharityDetailDto charityDetailDto)
        {

            using (var transaction = await dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var alreadyAddedCharity = await this.charityRepository.GetByEmailAsync(charityDetailDto.Email);
                    if (alreadyAddedCharity != null)
                    {
                        await transaction.RollbackAsync();
                        return new Response(HttpStatusCode.BadRequest, $"Email {charityDetailDto.Email} is already taken.");
                    }

                    (byte[], byte[]) hashSalt = GenerateHashAndSaltHelper.CreatePasswordHash(charityDetailDto.Password);

                    var charity = new Charity
                    {
                        Email = charityDetailDto.Email,
                        KycStatus = CharityKycStatus.Pending,
                        PasswordHash = hashSalt.Item2,
                        Salt = hashSalt.Item1,
                        Role = charityDetailDto.Role.ToUpper(),
                        EmailVerified = false,
                        PackageId = "0",
                        CountryCode = charityDetailDto.CountryCode,
                        CharityPhoneNumber = charityDetailDto.CharityPhoneNumber,
                    };

                    var addRegisterResult = await this.charityRepository.AddAsync(charity);

                    var verificationDetails = await this.emailVerificationRepository.GetByEmailAsync(charityDetailDto.Email);
                    if (verificationDetails != null)
                    {
                        await transaction.RollbackAsync();
                        return new Response(HttpStatusCode.BadRequest, $"Email {charityDetailDto.Email} is already taken.");
                    }

                    charity = await this.charityRepository.GetByEmailAsync(charityDetailDto.Email);
                    var emailVerificationCode = GenerateRandomNumberTokenHelper.GenerateRandomNumberToken(6);

                    var emailVerification = new EmailVerificationDetails
                    {
                        Email = charityDetailDto.Email,
                        Token = emailVerificationCode,
                        ExpiryTime = DateTime.UtcNow.AddMinutes(30),
                        CharityId = charity?.Id,
                        IsUsed = false
                    };

                    string finalHtml = GetEmailTemplate(emailVerificationCode);

                    //Send Email Work
                    var success = await this.emailMessageService.SendEmailAsync(
                    charityDetailDto.Email,
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

                    return new Response(HttpStatusCode.Created, $"Charity Registered Successfully and Email Verification Code send to your {EmailMaskHelper.MaskEmail(charityDetailDto.Email)}");
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

            //string htmlPath1 = Path.Combine(Directory.GetCurrentDirectory(), "Tabarru.Common", "HtmlTemplates", "EmailVerificationBody.html");
            string htmlPath = Path.Combine(AppContext.BaseDirectory, "HtmlTemplates", "EmailVerificationBody.html");

            //Console.WriteLine($" html path 1 : {htmlPath1}");
            Console.WriteLine($" html path : {htmlPath}");

            if (!File.Exists(htmlPath))
                throw new FileNotFoundException($"Email template not found at {htmlPath}");

            string htmlContent = File.ReadAllText(htmlPath);

            string finalHtml = htmlContent.Replace("{{CODE}}", emailVerificationCode);
            return finalHtml;
        }

        private static string GetForgotPasswordEmailTemplate(string code)
        {
            Console.WriteLine($" directory : {Directory.GetCurrentDirectory()}");

            //string htmlPath1 = Path.Combine(Directory.GetCurrentDirectory(), "Tabarru.Common", "HtmlTemplates", "EmailVerificationBody.html");
            string htmlPath = Path.Combine(AppContext.BaseDirectory, "HtmlTemplates", "ForgotPasswordBody.html");

            //Console.WriteLine($" html path 1 : {htmlPath1}");
            Console.WriteLine($" html path : {htmlPath}");

            if (!File.Exists(htmlPath))
                throw new FileNotFoundException($"Email template not found at {htmlPath}");

            string htmlContent = File.ReadAllText(htmlPath);

            string finalHtml = htmlContent.Replace("{{CODE}}", code);
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
            var charity = await charityRepository.GetCharityAllDetailsByIdAsync(ChairtyId);
            if (charity is null)
            {
                return new Response<CharityReadDto>(HttpStatusCode.OK, "Charity Not Found");
            }
            return new Response<CharityReadDto>(HttpStatusCode.OK, charity.MapToDto(), ResponseCode.Data);
        }

        public async Task<Response> UpdateCharityDetailsAsync(UpdateCharityDetailsDto request)
        {
            var charityKyc = await charityRepository.GetCharityKycDetailsApprovedAsync(request.CharityId);

            if (charityKyc == null)
                throw new Exception("Charity not found");

            charityKyc.FirstName = request.FirstName;
            charityKyc.LastName = request.LastName;
            charityKyc.CharityName = request.CharityName;
            charityKyc.CharityNumber = request.CharityNumber;
            charityKyc.CountryCode = request.CountryCode;

            if (charityKyc.CharityKycDocuments != null)
            {
                charityKyc.CharityKycDocuments.Logo = request.Logo;
            }

            var udpate = await charityKycRepository.UpdateAsync(charityKyc);

            if (!udpate)
            {
                return new Response(HttpStatusCode.BadRequest, "Charity update failed");
            }

            return new Response(HttpStatusCode.OK, "Charity update Successfully");
        }

        public async Task<Response> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
        {
            var charity = await charityRepository.GetByEmailAsync(forgotPasswordDto.Email);

            if (charity == null)
                return new Response(HttpStatusCode.NotFound, "Email not found");

            // Invalidate previous tokens 
            await passwordResetRepository.InvalidateExistingTokensAsync(charity.Id);

            var token = GenerateRandomNumberTokenHelper.GenerateRandomNumberToken(8);

            var resetToken = new PasswordResetToken
            {
                Id = Guid.NewGuid().ToString(),
                CharityId = charity.Id,
                Token = token,
                ExpiryTime = DateTime.UtcNow.AddMinutes(30),
                IsUsed = false
            };

            await passwordResetRepository.AddAsync(resetToken);

            // Prepare Email
            string finalHtml = GetForgotPasswordEmailTemplate(token);

            var success = await emailMessageService.SendEmailAsync(
                forgotPasswordDto.Email,
                "Password Reset",
                finalHtml
            );

            if (!success)
                return new Response(HttpStatusCode.InternalServerError, "Failed to send email");

            return new Response(HttpStatusCode.OK, $"Reset token sent to {EmailMaskHelper.MaskEmail(forgotPasswordDto.Email)}");
        }

        public async Task<Response> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            var tokenEntity = await passwordResetRepository.GetValidTokenAsync(resetPasswordDto.Token);

            if (tokenEntity == null)
                return new Response(HttpStatusCode.BadRequest, "Invalid or expired token");

            var charity = tokenEntity.Charity;

            (byte[], byte[]) hashSalt = GenerateHashAndSaltHelper.CreatePasswordHash(resetPasswordDto.NewPassword);

            charity.PasswordHash = hashSalt.Item2;
            charity.Salt = hashSalt.Item1;

            tokenEntity.IsUsed = true;

            await charityRepository.UpdateAsync(charity);
            await passwordResetRepository.UpdateAsync(tokenEntity);

            return new Response(HttpStatusCode.OK, "Password reset successful");
        }

        //public async Task<Response<LoginResponse>> GenerateRefreshToken(string token)
        //{

        //}
    }
}