using Tabarru.Common.Models;
using Tabarru.Services.Models;

namespace Tabarru.Services.IServices
{
    public interface IAnalyticsService
    {
        //Task<AnalyticsDataResponse> GetGiftAidSmallDonationsScheme(AnalyticsDetailsDto analyticsDetailsDto);
        Task<GiftAidSmallDonationsScheme> GetAnalyticsOfGiftAidSmallDonationsScheme(AnalyticsDetailsDto analyticsDetailsDto);
        Task<List<CampaignDonationSummary>> GetAnalyticsOfCampaignDonationSummary(AnalyticsDetailsDto analyticsDetailsDto);
        Task<RevenueDashboardGraphResponse> GetAnalyticsOfRevenueDashboardGraph(AnalyticsDetailsDto analyticsDetailsDto);
    }
}
