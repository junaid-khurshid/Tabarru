using Tabarru.Common.Models;
using Tabarru.Repositories.IRepository;
using Tabarru.Services.IServices;

namespace Tabarru.Services.Implementation
{
    public class DonationReportService : IDonationReportService
    {
        private readonly IDonationRepository donationRepository;

        public DonationReportService(IDonationRepository donationRepository)
        {
            this.donationRepository = donationRepository;
        }

        public Task<PagedResponse<DonationReportResponse>> GetAllTransactions(DateTime? start, DateTime? end, string charityId, int pageNumber, int pageSize)
        => donationRepository.GetAllTransactionsAsync(start, end, charityId, pageNumber, pageSize);

        public Task<PagedResponse<DonationReportResponse>> GetGiftAidTransactions(DateTime? start, DateTime? end, string charityId, int pageNumber, int pageSize)
        => donationRepository.GetGiftAidTransactionsAsync(start, end, charityId, pageNumber, pageSize);

        public Task<PagedResponse<MembershipTransactionResponse>> GetMembershipTransactions(DateTime? start, DateTime? end, string charityId, int pageNumber, int pageSize)
        => donationRepository.GetMembershipTransactionsAsync(start, end, charityId, pageNumber, pageSize);

        public Task<PagedResponse<StudentFormTransactionResponse>> GetStudentFormTransactions(DateTime? start, DateTime? end, string charityId, int pageNumber, int pageSize)
        => donationRepository.GetStudentFormTransactionsAsync(start, end, charityId, pageNumber, pageSize);

        public Task<PagedResponse<DonationReportResponse>> GetTransactionsWithoutGiftAid(DateTime? start, DateTime? end, string charityId, int pageNumber, int pageSize)
        => donationRepository.GetTransactionsWithoutGiftAidAsync(start, end, charityId, pageNumber, pageSize);
    }
}
