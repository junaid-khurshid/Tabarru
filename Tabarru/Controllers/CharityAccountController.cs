using Microsoft.AspNetCore.Mvc;
using Tabarru.Common.Models;
using Tabarru.RequestModels;
using Tabarru.Services.IServices;

namespace Tabarru.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharityAccountController : ControllerBase
    {
        private readonly ICharityAccountService charityAccountService;

        public CharityAccountController(ICharityAccountService charityAccountService)
        {
            this.charityAccountService = charityAccountService;
        }

        [HttpPost("register")]
        public async Task<Response> Register(RegisterCharityDetail registerCharityDetail)
        {
           return await this.charityAccountService.Register(registerCharityDetail.MapToDto());
        }

        [HttpPost("login")]
        public async Task<Response<LoginResponse>> LoginAsync(LoginRequest loginRequest)
        {
            return await this.charityAccountService.Login(loginRequest.MapToDto());
        }

        [HttpPost("confirmation/resend")]
        public async Task<Response> ReGenerateEmailVerificationToken([FromBody] string email)
        {
            return await this.charityAccountService.ReGenerateEmailVerificationTokenByEmail(email);
        }

        [HttpPost("confirmation")]
        public async Task<IActionResult> VerifyToken([FromBody] VerifyRequest request)
        {
            return await this.charityAccountService.VerifyToken(request.MapToDto());
        }


        [HttpPost("refresh")]
        public Response<LoginResponse> Refresh([FromBody] string token)
        {
            return null;
        }
    }
}
