using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tabarru.Common.Helper;
using Tabarru.Common.Models;
using Tabarru.RequestModels;
using Tabarru.Services.Implementation;
using Tabarru.Services.IServices;
using Tabarru.Services.Models;

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
        public async Task<Response> Register([FromBody] RegisterCharityRequest registerCharityDetail)
        {
            return await this.charityAccountService.Register(registerCharityDetail.MapToDto());
        }

        [HttpPost("login")]
        public async Task<Response<LoginResponse>> LoginAsync([FromBody] LoginRequest loginRequest)
        {
            return await this.charityAccountService.Login(loginRequest.MapToDto());
        }

        [HttpPost("confirmation/resend")]
        public async Task<Response> ReGenerateEmailVerificationToken([FromBody] string email)
        {
            return await this.charityAccountService.ReGenerateEmailVerificationTokenByEmail(email);
        }

        [HttpPost("confirmation/email")]
        public async Task<IActionResult> VerifyToken([FromBody] VerifyRequest request)
        {
            return await this.charityAccountService.VerifyToken(request.MapToDto());
        }

        [HttpPut("assignPackage")]
        public async Task<Response> AssignPackage([FromBody] CharityPackageUpdateRequest charityPackageUpdateRequest)
        {
            return await charityAccountService.AssignPackageAsync(charityPackageUpdateRequest.MaptoDto(TokenClaimHelper.GetId(User)));
        }

        [HttpGet("details")]
        public async Task<Response<CharityReadDto>> GetCharityDetails()
        {
            return await charityAccountService.GetCharityDetailsAsync(TokenClaimHelper.GetId(User));
        }

        /// <summary>
        ///  for testing purpose
        /// </summary>
        /// <returns></returns>
        //[Authorize(Roles = "ADMIN")]
        //[HttpGet("admin")]
        //public IActionResult AdminEndpoint()
        //{
        //    return Ok("You are an admin.");
        //}

        //[Authorize(Roles = "USER, ADMIN")]
        //[HttpGet("user")]
        //public IActionResult UserEndpoint()
        //{
        //    var id = TokenClaimHelper.GetId(User);
        //    var email = TokenClaimHelper.GetEmail(User);
        //    var role = TokenClaimHelper.GetRole(User);
        //    return Ok("You are a user.");
        //}


        [HttpPost("refresh")]
        public Response<LoginResponse> Refresh([FromBody] string token)
        {
            return null;
        }
    }
}
