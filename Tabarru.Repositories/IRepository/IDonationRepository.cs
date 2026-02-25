using Tabarru.Common.Models;

namespace Tabarru.Repositories.IRepository
{
    public interface IDonationRepository
    {
        Task<PagedResponse<DonationReportResponse>> GetAllTransactionsAsync(string charityId, int pageNumber, int pageSize);
        Task<PagedResponse<DonationReportResponse>> GetGiftAidTransactionsAsync(string charityId, int pageNumber, int pageSize);
        Task<PagedResponse<DonationReportResponse>> GetTransactionsWithoutGiftAidAsync(string charityId, int pageNumber, int pageSize);
    }

}
