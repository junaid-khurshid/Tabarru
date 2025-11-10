using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tabarru.Common.Helper;
using Tabarru.Common.Models;
using Tabarru.RequestModels;
using Tabarru.Services.IServices;
using Tabarru.Services.Models;

namespace Tabarru.Controllers
{
    [Authorize(Policy = "KycApprovedOnly", Roles = "USER,ADMIN")]
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            this.paymentService = paymentService;
        }

        [HttpPost("save")]
        public async Task<Response> SavePayment([FromBody] PaymentSaveRequest dto)
        {
            return await paymentService.SavePayment(dto.MapToDto(TokenClaimHelper.GetId(User)));

        }

        [HttpGet("all")]
        public async Task<Response<List<PaymentReadDetailDto>>> GetByCharityId()
        {
            return await paymentService.GetByCharityIdAsync(TokenClaimHelper.GetId(User));
        }

        [HttpGet("searchByCampaignOrTemplate")]
        public async Task<Response<List<PaymentReadDetailDto>>> GetByCampaignOrTemplate([FromQuery] string? campaignId, [FromQuery] string? templateId)
        {
            return await paymentService.GetByCampaignOrTemplateIdAsync(campaignId, templateId);
        }
    }
}
