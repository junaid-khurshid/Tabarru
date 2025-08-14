using Microsoft.AspNetCore.Mvc;
using Tabarru.Common.Helper;
using Tabarru.Common.Models;
using Tabarru.RequestModels;
using Tabarru.Services.IServices;
using Tabarru.Services.Models;

namespace Tabarru.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignController : ControllerBase
    {
        private readonly ICampaignService campaignService;

        public CampaignController(ICampaignService campaignService)
        {
            this.campaignService = campaignService;
        }
        [HttpPost]
        public async Task<Response> Create([FromForm] CampaignCreateRequest campaignCreateRequest)
        {
            return await campaignService.CreateAsync(campaignCreateRequest.MapToDto(TokenClaimHelper.GetId(User)));
        }

        [HttpGet("/{id:guid}")]
        public async Task<Response<CampaignReadDto>> GetById(Guid id)
        {
            return await campaignService.GetByIdAsync(id.ToString());
        }

        [HttpGet]
        public async Task<Response<IList<CampaignReadDto>>> GetAll()
        {
            return await campaignService.GetAllByCharityIDAsync(TokenClaimHelper.GetId(User));
        }

        [HttpPut("/status")]
        public async Task<IActionResult> UpdateStatus([FromBody] CampaignUpdateStatusRequest dto)
        {
            return await campaignService.UpdateStatusAsync(dto.MapToDto(TokenClaimHelper.GetId(User)));
        }
    }
}