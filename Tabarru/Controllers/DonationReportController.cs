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
        public async Task<Response<IList<DonationReportResponse>>> GetAllTransactions()
        {
            var res = await donationReportService.GetAllTransactions(TokenClaimHelper.GetId(User));

            return new Response<IList<DonationReportResponse>>(HttpStatusCode.OK, res, ResponseCode.Data);
        }

        [HttpGet("gift-aid-transactions")]
        public async Task<Response<IList<DonationReportResponse>>> GetGiftAidTransactions()
        {
            var res = await donationReportService.GetGiftAidTransactions(TokenClaimHelper.GetId(User));

            return new Response<IList<DonationReportResponse>>(HttpStatusCode.OK, res, ResponseCode.Data);
        }

        [HttpGet("transactions-without-gift-aid")]
        public async Task<Response<IList<DonationReportResponse>>> GetTransactionsWithoutGiftAid()
        {
            var res = await donationReportService.GetTransactionsWithoutGiftAid(TokenClaimHelper.GetId(User));

            return new Response<IList<DonationReportResponse>>(HttpStatusCode.OK, res, ResponseCode.Data);
            
        }
    }
}
