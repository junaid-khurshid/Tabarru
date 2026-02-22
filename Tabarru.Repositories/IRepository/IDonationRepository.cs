using Tabarru.Common.Models;

namespace Tabarru.Repositories.IRepository
{
    public interface IDonationRepository
    {
        Task<List<DonationReportResponse>> GetAllTransactionsAsync(string charityId);
        Task<List<DonationReportResponse>> GetGiftAidTransactionsAsync(string charityId);
        Task<List<DonationReportResponse>> GetTransactionsWithoutGiftAidAsync(string charityId);
    }

}
