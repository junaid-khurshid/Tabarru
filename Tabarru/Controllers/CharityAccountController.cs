using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Tabarru.Common.Models;
using Tabarru.RequestModels;
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
        public async Task<Response> Register(RegisterCharityDetail registerCharityDetail)
        {
            var result = await this.charityAccountService.Register(registerCharityDetail.MapToDto());

            if (result.Status)
            {
                return new Response(HttpStatusCode.Created, result.Message);
            }

            return new Response(HttpStatusCode.BadRequest, result.Message);
        }

        [HttpPost("login")]
        public async Task<Response<LoginResponse>> LoginAsync(LoginRequest loginRequest)
        {
            return await this.charityAccountService.Login(loginRequest.MapToDto());
        }

        [HttpPost("refresh")]
        public Response<LoginResponse> Refresh([FromBody] string token)
        {
            return null;
        }
    }
}
