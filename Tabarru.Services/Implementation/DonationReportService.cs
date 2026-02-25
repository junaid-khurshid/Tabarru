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

        public Task<PagedResponse<DonationReportResponse>> GetAllTransactions(string charityId, int pageNumber, int pageSize)
        => donationRepository
        .GetAllTransactionsAsync(
        charityId,
        pageNumber,
        pageSize);



        public Task<PagedResponse<DonationReportResponse>>
        GetGiftAidTransactions(string charityId, int pageNumber, int pageSize) =>
        donationRepository
        .GetGiftAidTransactionsAsync(
        charityId,
        pageNumber,
        pageSize);



        public Task<PagedResponse<DonationReportResponse>> GetTransactionsWithoutGiftAid(string charityId, int pageNumber, int pageSize)
        => donationRepository
        .GetTransactionsWithoutGiftAidAsync(
        charityId,
        pageNumber,
        pageSize);
    }
}
