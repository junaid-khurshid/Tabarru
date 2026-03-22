using Tabarru.Common.Models;

namespace Tabarru.Services.IServices
{
    public interface IDonationReportService
    {
        Task<PagedResponse<DonationReportResponse>> GetAllTransactions(DateTime? start, DateTime? end, string charityId, int pageNumber, int pageSize);
        Task<PagedResponse<DonationReportResponse>> GetGiftAidTransactions(DateTime? start, DateTime? end, string charityId, int pageNumber, int pageSize);
        Task<PagedResponse<DonationReportResponse>> GetTransactionsWithoutGiftAid(DateTime? start, DateTime? end, string charityId, int pageNumber, int pageSize);
        Task<PagedResponse<MembershipTransactionResponse>> GetMembershipTransactions(DateTime? start, DateTime? end, string charityId, int pageNumber, int pageSize);
        Task<PagedResponse<StudentFormTransactionResponse>> GetStudentFormTransactions(DateTime? start, DateTime? end, string charityId, int pageNumber, int pageSize);
    }
}
