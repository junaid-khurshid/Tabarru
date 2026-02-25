using Microsoft.AspNetCore.Mvc;
using System.Net;
using Tabarru.Common.Enums;
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
        public async Task<PagedResponse<DonationReportResponse>> GetAllTransactions(
        int pageNumber = 1,
        int pageSize = 10)
        {
            return await donationReportService
                        .GetAllTransactions(
                            TokenClaimHelper.GetId(User),
                            pageNumber,
                            pageSize);
        }


        [HttpGet("gift-aid-transactions")]
        public async Task<PagedResponse<DonationReportResponse>> GetGiftAidTransactions(
        int pageNumber = 1,
        int pageSize = 10)
        {
            return await donationReportService
                        .GetGiftAidTransactions(
                            TokenClaimHelper.GetId(User),
                            pageNumber,
                            pageSize);
        }


        [HttpGet("transactions-without-gift-aid")]
        public async Task<PagedResponse<DonationReportResponse>> GetTransactionsWithoutGiftAid(
        int pageNumber = 1,
        int pageSize = 10)
        {
            return await donationReportService
            .GetTransactionsWithoutGiftAid(
                TokenClaimHelper.GetId(User),
                pageNumber,
                pageSize);
        }
    }
}
