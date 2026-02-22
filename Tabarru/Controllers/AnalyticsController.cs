using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tabarru.Common.Helper;
using Tabarru.Common.Models;
using Tabarru.RequestModels;
using Tabarru.Services.IServices;

namespace Tabarru.Controllers
{
    [Authorize(Policy = "KycApprovedOnly", Roles = "USER,ADMIN")]
    [Route("api/[controller]")]
    [ApiController]
    public class AnalyticsController : ControllerBase
    {
        private readonly IAnalyticsService analyticsService;

        public AnalyticsController(IAnalyticsService analyticsService)
        {
            this.analyticsService = analyticsService;
        }

        //[HttpPost]
        //public async Task<AnalyticsDataResponse> GetAnalytics([FromBody] AnalyticsRequest analyticsRequest)
        //{
        //    return await analyticsService.GetGiftAidSmallDonationsScheme(analyticsRequest.MapToDto(TokenClaimHelper.GetId(User)));
        //}

        [HttpPost("GiftAidSmallDonationsScheme")]
        public async Task<GiftAidSmallDonationsScheme> GetAnalyticsOfGiftAidSmallDonationsScheme([FromBody] AnalyticsRequest analyticsRequest)
        {
            return await analyticsService.GetAnalyticsOfGiftAidSmallDonationsScheme(analyticsRequest.MapToDto(TokenClaimHelper.GetId(User)));
        }

        [HttpPost("CampaignDonationSummary")]
        public async Task<List<CampaignDonationSummary>> GetAnalyticsOfCampaignDonationSummary([FromBody] AnalyticsRequest analyticsRequest)
        {
            return await analyticsService.GetAnalyticsOfCampaignDonationSummary(analyticsRequest.MapToDto(TokenClaimHelper.GetId(User)));
        }

        [HttpPost("RevenueDashboardGraph")]
        public async Task<RevenueDashboardGraphResponse> GetAnalyticsOfRevenueDashboardGraph([FromBody] AnalyticsRequest analyticsRequest)
        {
            return await analyticsService.GetAnalyticsOfRevenueDashboardGraph(analyticsRequest.MapToDto(TokenClaimHelper.GetId(User)));
        }
    }
}
