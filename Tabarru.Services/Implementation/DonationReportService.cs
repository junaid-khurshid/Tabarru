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

        public Task<List<DonationReportResponse>> GetAllTransactions(string charityId) =>
        donationRepository.GetAllTransactionsAsync(charityId);

        public Task<List<DonationReportResponse>> GetGiftAidTransactions(string charityId) =>
            donationRepository.GetGiftAidTransactionsAsync(charityId);

        public Task<List<DonationReportResponse>> GetTransactionsWithoutGiftAid(string charityId) =>
            donationRepository.GetTransactionsWithoutGiftAidAsync(charityId);
    }
}
