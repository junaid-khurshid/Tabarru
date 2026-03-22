using Tabarru.Common.Models;

namespace Tabarru.Repositories.IRepository
{
    public interface IDonationRepository
    {
        Task<PagedResponse<DonationReportResponse>> GetAllTransactionsAsync(DateTime? start, DateTime? end, string charityId, int pageNumber, int pageSize);
        Task<PagedResponse<DonationReportResponse>> GetGiftAidTransactionsAsync(DateTime? start, DateTime? end, string charityId, int pageNumber, int pageSize);
        Task<PagedResponse<DonationReportResponse>> GetTransactionsWithoutGiftAidAsync(DateTime? start, DateTime? end, string charityId, int pageNumber, int pageSize);
        Task<PagedResponse<MembershipTransactionResponse>> GetMembershipTransactionsAsync(DateTime? start, DateTime? end, string charityId, int pageNumber, int pageSize);
        Task<PagedResponse<StudentFormTransactionResponse>> GetStudentFormTransactionsAsync(DateTime? start, DateTime? end, string charityId, int pageNumber, int pageSize);
        Task<List<PaymentDetailResponse>> GetTodayTransactionsAsync(string charityId);
    }

}
