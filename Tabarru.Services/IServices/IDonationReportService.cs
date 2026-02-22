using Tabarru.Common.Models;

namespace Tabarru.Services.IServices
{
    public interface IDonationReportService
    {
        Task<List<DonationReportResponse>> GetAllTransactions(string charityId);
        Task<List<DonationReportResponse>> GetGiftAidTransactions(string charityId);
        Task<List<DonationReportResponse>> GetTransactionsWithoutGiftAid(string charityId);
    }
}
