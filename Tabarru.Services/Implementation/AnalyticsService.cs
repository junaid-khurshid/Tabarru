using Tabarru.Common.Models;
using Tabarru.Repositories.IRepository;
using Tabarru.Services.IServices;
using Tabarru.Services.Models;

namespace Tabarru.Services.Implementation
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IPaymentRepository paymentRepository;

        public AnalyticsService(IPaymentRepository paymentRepository)
        {
            this.paymentRepository = paymentRepository;
        }

        //public async Task<AnalyticsDataResponse> GetGiftAidSmallDonationsScheme(AnalyticsDetailsDto analyticsDetailsDto)
        //{
        //    //DateTime? start = null;
        //    //DateTime? end = null;

        //    //if (analyticsDetailsDto.StartDate != default && analyticsDetailsDto.EndDate != default)
        //    //{
        //    //    start = analyticsDetailsDto.StartDate.ToDateTime(TimeOnly.MinValue);
        //    //    end = analyticsDetailsDto.EndDate.ToDateTime(TimeOnly.MaxValue);
        //    //}

        //    var response = new AnalyticsDataResponse
        //    {

        //        GiftAidSmallDonationsSchemes = await paymentRepository.GetGiftAidSummary(analyticsDetailsDto.StartDate, analyticsDetailsDto.EndDate, analyticsDetailsDto.CharityId),
        //        CampaignSummaries = await paymentRepository.GetCampaignGraphList(analyticsDetailsDto.StartDate, analyticsDetailsDto.EndDate, analyticsDetailsDto.CharityId),
        //        RevenueGraph = await paymentRepository.GetRevenueGraphList(analyticsDetailsDto.StartDate, analyticsDetailsDto.EndDate, analyticsDetailsDto.CharityId)
        //    };

        //    return response;
        //}

        public async Task<GiftAidSmallDonationsScheme> GetAnalyticsOfGiftAidSmallDonationsScheme(AnalyticsDetailsDto analyticsDetailsDto)
        {
            return await paymentRepository.GetGiftAidSummary(analyticsDetailsDto.StartDate, analyticsDetailsDto.EndDate, analyticsDetailsDto.CharityId);
        }

        public async Task<List<CampaignDonationSummary>> GetAnalyticsOfCampaignDonationSummary(AnalyticsDetailsDto analyticsDetailsDto)
        {
            return await paymentRepository.GetCampaignGraphList(analyticsDetailsDto.StartDate, analyticsDetailsDto.EndDate, analyticsDetailsDto.CharityId);
        }

        public async Task<RevenueDashboardGraphResponse> GetAnalyticsOfRevenueDashboardGraph(AnalyticsDetailsDto analyticsDetailsDto)
        {
            return await paymentRepository.GetRevenueGraphList(analyticsDetailsDto.StartDate, analyticsDetailsDto.EndDate, analyticsDetailsDto.CharityId);
        }
    }
}
