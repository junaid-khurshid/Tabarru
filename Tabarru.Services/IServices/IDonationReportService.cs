using Tabarru.Common.Models;

namespace Tabarru.Services.IServices
{
    public interface IDonationReportService
    {
        Task<PagedResponse<DonationReportResponse>> GetAllTransactions(string charityId, int pageNumber, int pageSize);
        Task<PagedResponse<DonationReportResponse>> GetGiftAidTransactions(string charityId, int pageNumber, int pageSize);
        Task<PagedResponse<DonationReportResponse>> GetTransactionsWithoutGiftAid(string charityId, int pageNumber, int pageSize);
    }
}
