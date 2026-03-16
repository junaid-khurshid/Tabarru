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
    public class MobileController : ControllerBase
    {
        private readonly ICampaignService campaignService;
        private readonly ITemplateService templateService;
        private readonly ICharityAccountService charityAccountService;
        private readonly IPaymentService paymentService;

        public MobileController(ICampaignService campaignService,
            ITemplateService templateService,
            ICharityAccountService charityAccountService,
            IPaymentService paymentService)
        {
            this.campaignService = campaignService;
            this.templateService = templateService;
            this.charityAccountService = charityAccountService;
            this.paymentService = paymentService;
        }

        [HttpPost("login")]
        public async Task<Response<LoginResponse>> LoginAsync([FromBody] LoginRequest loginRequest)
        {
            return await this.charityAccountService.Login(loginRequest.MapToDto());
        }

        [HttpGet("campaign/{id:guid}")]
        public async Task<Response<CampaignReadDto>> GetById(Guid id)
        {
            return await campaignService.GetByIdAsync(id.ToString());
        }

        [HttpGet("campaign/all")]
        public async Task<Response<IList<CampaignReadDto>>> GetCampaignAll()
        {
            return await campaignService.GetAllByCharityIDAsync(TokenClaimHelper.GetId(User));
        }

        [HttpGet("templates/all")]
        public async Task<Response<IEnumerable<TemplateReadDto>>> GetTemplatesAll()
        {
            return await templateService.GetAllTemplatesAsync(TokenClaimHelper.GetId(User));
        }

        [HttpGet("templates/{id}")]
        public async Task<Response<TemplateReadDto>> GetById(string id)
        {
            return await templateService.GetTemplateByIdAsync(id);
        }

        [HttpGet("get-today-transactions")]
        public async Task<List<PaymentDetailResponse>> GetAllTransactions()
        {
            return await paymentService.GetTodayTransactionsAsync(TokenClaimHelper.GetId(User));
        }

    }
}
