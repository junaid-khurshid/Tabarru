using Tabarru.Common.Models;
using Tabarru.Repositories.Models;

namespace Tabarru.Repositories.IRepository
{
    public interface IPaymentRepository
    {
        Task<bool> AddAsync(PaymentDetail paymentDetail);
        Task<PaymentDetail> GetByIdAsync(string PaymentId);
        Task<bool> UpdateAsync(PaymentDetail paymentDetail);

        Task<IEnumerable<PaymentDetail>> GetByCharityIdAsync(string charityId);
        Task<IEnumerable<PaymentDetail>> GetByCampaignOrTemplateIdAsync(string? campaignId, string? templateId);
        //Task<List<CampaignDonationSummary>> GetTodayCampaignSummary();
        //Task<DonationSummary> GetDonationSummary();

        Task<GiftAidSmallDonationsScheme> GetGiftAidSummary(DateTime? start, DateTime? end, string charityId);
        Task<List<CampaignDonationSummary>> GetCampaignGraphList(DateTime? start, DateTime? end, string charityId);
        Task<RevenueDashboardGraphResponse> GetRevenueGraphList(DateTime? start, DateTime? end, string charityId);
    }
}
