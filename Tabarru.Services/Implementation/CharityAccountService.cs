using Microsoft.Extensions.Configuration;
using System.Net;
using Tabarru.Common.Enums;
using Tabarru.Common.Helper;
using Tabarru.Common.Models;
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
        private readonly IConfiguration configuration;

        public CharityAccountService(ICharityRepository charityRepository,
            IEmailVerificationRepository emailVerificationRepository,
            IConfiguration configuration)
        {
            this.charityRepository = charityRepository;
            this.emailVerificationRepository = emailVerificationRepository;
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
                return new Response<LoginResponse>(HttpStatusCode.NotFound, new LoginResponse { EmailConfirmed = false }, "Sorry! Cahrity does not exists with this email address..", ResponseCode.Errors);

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

            return new Response<LoginResponse>(HttpStatusCode.NotFound, new LoginResponse
            {
                EmailConfirmed = true,
                AccessToken = token.Item1,
                RefreshToken = refreshToken.Item1,
                ExpiresIn = token.Item2
            },
                "Charity Not Found.", ResponseCode.Errors);
        }

        public async Task<Response> Register(CharityDetailDto dto)
        {
            var alreadyAddedCharity = await this.charityRepository.GetByEmailAsync(dto.Email);
            if (alreadyAddedCharity != null)
                return new Response(HttpStatusCode.BadRequest, $"Email {dto.Email} is already taken.");


            (byte[], byte[]) hashSalt = GenerateHashAndSaltHelper.CreatePasswordHash(dto.Password);

            var charity = new Charity
            {
                Email = dto.Email,
                KycStatus = false,
                PasswordHash = hashSalt.Item2,
                Salt = hashSalt.Item1,
                Role = dto.Role,
                EmailVerified = false,
            };

            var verificationDetails = await this.emailVerificationRepository.GetByEmailAsync(dto.Email);
            if (verificationDetails != null)
            {
                return new Response(HttpStatusCode.BadRequest, $"Email {dto.Email} is already taken.");
            }

            var token = GenerateRandomNumberTokenHelper.GenerateRandomNumberToken(6);

            var verification = new EmailVerificationDetails
            {
                Email = dto.Email,
                Token = token,
                ExpiryTime = DateTime.UtcNow.AddMinutes(30)
            };

            //Send Email Work

            var addEmailVerify = await this.emailVerificationRepository.AddAsync(verification);
            var addRegisterResult = await this.charityRepository.AddAsync(charity);

            if (!addRegisterResult || !addEmailVerify)
                return new Response(HttpStatusCode.BadRequest, "Charity Registration Failed");

            return new Response(HttpStatusCode.Created, "Charity Registered Successfully");
        }


        public async Task<Response> ReGenerateEmailVerificationTokenByEmail(string email)
        {
            var verificationDetails = await this.emailVerificationRepository.GetByEmailAsync(email);



            var token = GenerateRandomNumberTokenHelper.GenerateRandomNumberToken(6);

            var verification = new EmailVerificationDetails
            {
                Email = email,
                Token = token,
                ExpiryTime = DateTime.UtcNow.AddMinutes(30)
            };

            var res = await this.emailVerificationRepository.AddAsync(verification);

            if (!res)
            {
                return new Response(HttpStatusCode.Created, "Confirmation email failed.");
            }

            return new Response(HttpStatusCode.BadRequest, "Confirmation email sent successfully.");

            //if (!res)
            //    return new ResultData(false, "Confirmation email failed.");

            //return new ResultData(res, "Confirmation email sent successfully.");
        }

        public async Task<Response> VerifyToken(VerifyRequestDto request)
        {
            var verificationDetails = await this.emailVerificationRepository.GetByEmailAsync(request.Email);

            if (verificationDetails == null || verificationDetails.ExpiryTime < DateTime.UtcNow)
                return new Response(HttpStatusCode.BadRequest, "Invalid or expired token.");

            verificationDetails.IsUsed = true;

            var res = await this.emailVerificationRepository.UpdateAsync(verificationDetails);

            if (!res)
            {
                return new Response(HttpStatusCode.BadRequest, "Email verification failed.");
            }

            return new Response(HttpStatusCode.Accepted, "Email verified successfully.");
        }

        //public async Task<Response<LoginResponse>> GenerateRefreshToken(string token)
        //{

        //}
    }
}
