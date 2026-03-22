using Microsoft.AspNetCore.Mvc;
using Tabarru.Common.Helper;
using Tabarru.Common.Models;
using Tabarru.Services.IServices;

namespace Tabarru.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonationReportController : ControllerBase
    {
        private readonly IDonationReportService donationReportService;

        public DonationReportController(IDonationReportService donationReportService)
        {
            this.donationReportService = donationReportService;
        }

        [HttpGet("all-transactions")]
        public async Task<PagedResponse<DonationReportResponse>> GetAllTransactions(DateTime? startDate, DateTime? endDate, int pageNumber = 1, int pageSize = 10)
        {
            return await donationReportService.GetAllTransactions(startDate, endDate, TokenClaimHelper.GetId(User), pageNumber, pageSize);
        }


        [HttpGet("gift-aid-transactions")]
        public async Task<PagedResponse<DonationReportResponse>> GetGiftAidTransactions(DateTime? startDate, DateTime? endDate, int pageNumber = 1, int pageSize = 10)
        {
            return await donationReportService.GetGiftAidTransactions(startDate, endDate, TokenClaimHelper.GetId(User), pageNumber, pageSize);
        }


        [HttpGet("transactions-without-gift-aid")]
        public async Task<PagedResponse<DonationReportResponse>> GetTransactionsWithoutGiftAid(DateTime? startDate, DateTime? endDate, int pageNumber = 1, int pageSize = 10)
        {
            return await donationReportService.GetTransactionsWithoutGiftAid(startDate, endDate, TokenClaimHelper.GetId(User), pageNumber, pageSize);
        }

        [HttpGet("membership-transactions")]
        public async Task<PagedResponse<MembershipTransactionResponse>> GetMembershipTransactions(DateTime? startDate, DateTime? endDate, int pageNumber = 1, int pageSize = 10)
        {
            return await donationReportService.GetMembershipTransactions(startDate, endDate, TokenClaimHelper.GetId(User), pageNumber, pageSize);
        }

        [HttpGet("student-form-transactions")]
        public async Task<PagedResponse<StudentFormTransactionResponse>> GetStudentFormTransactions(DateTime? startDate, DateTime? endDate, int pageNumber = 1, int pageSize = 10)
        {
            return await donationReportService.GetStudentFormTransactions(startDate, endDate, TokenClaimHelper.GetId(User), pageNumber, pageSize);
        }

    }
}
