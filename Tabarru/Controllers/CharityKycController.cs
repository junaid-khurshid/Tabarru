using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tabarru.Common.Helper;
using Tabarru.Common.Models;
using Tabarru.Repositories.Models;
using Tabarru.RequestModels;
using Tabarru.Services.IServices;
using Tabarru.Services.Models;

namespace Tabarru.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "USER, ADMIN")]

    public class CharityKycController : ControllerBase
    {
        private readonly ICharityKycService _kycService;

        public CharityKycController(ICharityKycService kycService)
        {
            _kycService = kycService;
        }

        [HttpPost("submit")]
        public async Task<Response> SubmitKyc([FromForm] CharityKycSubmitRequest dto)
        {
            return await _kycService.SubmitKycAsync(TokenClaimHelper.GetId(User), dto.MapToDto());
        }


        [HttpGet("admin/all")]
        public async Task<Response<IList<Charity>>> GetAllCharities()
        {
            return await _kycService.GetAllCharitiesForAdminAsync();
        }

        [HttpPut("admin/update-status")]
        public async Task<Response> UpdateKycStatus([FromBody] AdminKycUpdateDto dto)
        {
            return await _kycService.UpdateKycStatusAsync(dto);
        }
    }
}
