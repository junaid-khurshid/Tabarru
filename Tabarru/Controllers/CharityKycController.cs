using Microsoft.AspNetCore.Mvc;
using Tabarru.Common.Models;
using Tabarru.Repositories.Models;
using Tabarru.Services.IServices;
using Tabarru.Services.Models;

namespace Tabarru.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharityKycController : ControllerBase
    {
        private readonly ICharityKycService _kycService;

        public CharityKycController(ICharityKycService kycService)
        {
            _kycService = kycService;
        }

        [HttpPost("{charityId}/submit")]
        public async Task<Response> SubmitKyc(string charityId, [FromBody] CharityKycDto dto)
        {
            return await _kycService.SubmitKycAsync(charityId, dto);
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
