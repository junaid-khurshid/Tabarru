using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Tabarru.Common.Enums;
using Tabarru.Common.Models;
using Tabarru.Repositories.DatabaseContext;
using Tabarru.Repositories.IRepository;
using Tabarru.Repositories.Models;

namespace Tabarru.Repositories.Implementation
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly DbStorageContext dbStorageContext;

        public PaymentRepository(DbStorageContext dbStorageContext)
        {
            this.dbStorageContext = dbStorageContext;
        }
        public async Task<bool> AddAsync(PaymentDetail paymentDetail)
        {
            dbStorageContext.PaymentDetails.Add(paymentDetail);
            return await dbStorageContext.SaveChangesAsync() > 0;
        }

        public async Task<PaymentDetail> GetByIdAsync(string PaymentId)
        {
            return await dbStorageContext.PaymentDetails.FirstOrDefaultAsync(x => x.Id.Equals(PaymentId));
        }

        public async Task<IEnumerable<PaymentDetail>> GetByCharityIdAsync(string charityId)
        {
            return await dbStorageContext.PaymentDetails
                .Where(p => p.CharityId == charityId && !p.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<PaymentDetail>> GetByCampaignOrTemplateIdAsync(string? campaignId, string? templateId)
        {
            return await dbStorageContext.PaymentDetails
                .Where(p => !p.IsDeleted &&
                            (p.CampaignId == campaignId || p.TemplateId == templateId))
                .ToListAsync();
        }

        public async Task<bool> UpdateAsync(PaymentDetail paymentDetail)
        {
            dbStorageContext.PaymentDetails.Update(paymentDetail);
            return await dbStorageContext.SaveChangesAsync() > 0;
        }

        //public async Task<List<CampaignDonationSummary>> GetTodayCampaignSummary()
        //{
        //    var today = DateTime.UtcNow.Date;
        //    var monthStart = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
        //    var yearStart = new DateTime(DateTime.UtcNow.Year, 1, 1);

        //    var result = await dbStorageContext.PaymentDetails
        //        .Where(x => !x.IsDeleted && x.Status == PaymentStatus.Succeeded)
        //        .GroupBy(x => x.CampaignId)
        //        .Select(g => new CampaignDonationSummary
        //        {
        //            CampaignId = g.Key,

        //            TodayAmount = g
        //                .Where(x => x.PaymentDateTime >= today)
        //                .Sum(x => (decimal?)x.Amount) ?? 0,

        //            ThisMonthAmount = g
        //                .Where(x => x.PaymentDateTime >= monthStart)
        //                .Sum(x => (decimal?)x.Amount) ?? 0,

        //            ThisYearAmount = g
        //                .Where(x => x.PaymentDateTime >= yearStart)
        //                .Sum(x => (decimal?)x.Amount) ?? 0
        //        })
        //        .ToListAsync();

        //    return result;
        //}

        //public async Task<DonationSummary> GetDonationSummary()
        //{
        //    var today = DateTime.UtcNow.Date;
        //    var monthStart = new DateTime(today.Year, today.Month, 1);
        //    var yearStart = new DateTime(today.Year, 1, 1);

        //    var payments = dbStorageContext.PaymentDetails
        //        .Where(x => !x.IsDeleted && x.Status == PaymentStatus.Succeeded);

        //    var response = new DonationSummary
        //    {
        //        TotalRevenues = new TotalRevenue
        //        {
        //            TodayTotalRevenue = await payments
        //                .Where(x => x.PaymentDateTime >= today)
        //                .SumAsync(x => (decimal?)x.Amount) ?? 0,

        //            ThisMonthTotalRevenue = await payments
        //                .Where(x => x.PaymentDateTime >= monthStart)
        //                .SumAsync(x => (decimal?)x.Amount) ?? 0,

        //            ThisYearTotalRevenue = await payments
        //                .Where(x => x.PaymentDateTime >= yearStart)
        //                .SumAsync(x => (decimal?)x.Amount) ?? 0
        //        },

        //        GiftAidTotalRevenues = new GiftAidTotalRevenue
        //        {
        //            TodayGiftAidRevenue = await payments
        //                .Where(x => x.PaymentDateTime >= today && x.IsGiftAid)
        //                .SumAsync(x => (decimal?)x.Amount) ?? 0,

        //            ThisMonthGiftAidRevenue = await payments
        //                .Where(x => x.PaymentDateTime >= monthStart && x.IsGiftAid)
        //                .SumAsync(x => (decimal?)x.Amount) ?? 0,

        //            ThisYearGiftAidRevenue = await payments
        //                .Where(x => x.PaymentDateTime >= yearStart && x.IsGiftAid)
        //                .SumAsync(x => (decimal?)x.Amount) ?? 0
        //        }
        //    };

        //    return response;
        //}

        public async Task<GiftAidSmallDonationsScheme> GetGiftAidSummary(DateTime? start, DateTime? end, string charityId)
        {
            var query = dbStorageContext.PaymentDetails
                .Where(x => x.CharityId == charityId && !x.IsDeleted && x.Status == PaymentStatus.Succeeded);

            if (start.HasValue && end.HasValue && start != default && end != default)
            {
                query = query.Where(x => x.PaymentDateTime >= start.Value &&
                                         x.PaymentDateTime <= end.Value);
            }

            return new GiftAidSmallDonationsScheme
            {
                TotalDonors = await query.Select(x => x.CustomerId).Distinct().CountAsync(),
                TotalDonation = (int)(await query.SumAsync(x => (decimal?)x.Amount) ?? 0),
                GiftAndDonations = (int)(await query
                    .Where(x => x.IsGiftAid)
                    .SumAsync(x => (decimal?)x.Amount) ?? 0),
                GiftSmallDonations = (int)(await query
                    .Where(x => x.IsGiftAid && x.Amount <= 30)
                    .SumAsync(x => (decimal?)x.Amount) ?? 0)
            };
        }

        public async Task<List<CampaignDonationSummary>> GetCampaignGraphList(DateTime? start, DateTime? end, string charityId)
        {
            var baseQuery = dbStorageContext.PaymentDetails
                .Where(x => x.CharityId == charityId &&
                            !x.IsDeleted &&
                            x.Status == PaymentStatus.Succeeded);

            // CUSTOM RANGE MODE
            if (start.HasValue && end.HasValue)
            {
                var startDate = start.Value.Date;
                var endDate = end.Value.Date.AddDays(1); // exclusive

                return await baseQuery
                    .Where(x => x.PaymentDateTime >= startDate &&
                                x.PaymentDateTime < endDate)
                    .Join(dbStorageContext.Campaigns,
                          payment => payment.CampaignId,
                          campaign => campaign.Id,
                          (payment, campaign) => new { payment, campaign })
                    .GroupBy(x => new { x.payment.CampaignId, x.campaign.Name })
                    .Select(g => new CampaignDonationSummary
                    {
                        CampaignId = g.Key.CampaignId,
                        CampaignName = g.Key.Name,

                        TodayAmount = g.Sum(x => x.payment.Amount),
                        ThisWeekAmount = 0,
                        ThisMonthAmount = 0
                    })
                    .ToListAsync();
            }

            // (Today / Week / Month)

            var today = DateTime.UtcNow.Date;

            // Monday as week start
            var diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
            var weekStart = today.AddDays(-diff);

            var monthStart = new DateTime(today.Year, today.Month, 1);

            return await baseQuery
                .Join(dbStorageContext.Campaigns,
                      payment => payment.CampaignId,
                      campaign => campaign.Id,
                      (payment, campaign) => new { payment, campaign })
                .GroupBy(x => new { x.payment.CampaignId, x.campaign.Name })
                .Select(g => new CampaignDonationSummary
                {
                    CampaignId = g.Key.CampaignId,
                    CampaignName = g.Key.Name,

                    TodayAmount = g
                        .Where(x => x.payment.PaymentDateTime >= today)
                        .Sum(x => (decimal?)x.payment.Amount) ?? 0,

                    ThisWeekAmount = g
                        .Where(x => x.payment.PaymentDateTime >= weekStart)
                        .Sum(x => (decimal?)x.payment.Amount) ?? 0,

                    ThisMonthAmount = g
                        .Where(x => x.payment.PaymentDateTime >= monthStart)
                        .Sum(x => (decimal?)x.payment.Amount) ?? 0
                })
                .ToListAsync();
        }

        public async Task<RevenueDashboardGraphResponse> GetRevenueGraphList(
            DateTime? start,
            DateTime? end,
            string charityId)
        {
            var baseQuery = dbStorageContext.PaymentDetails
                .Where(x =>
                    x.CharityId == charityId &&
                    !x.IsDeleted &&
                    x.Status == PaymentStatus.Succeeded);

            var response = new RevenueDashboardGraphResponse();

            // =====================
            // CUSTOM DATE FILTER
            // =====================

            if (start.HasValue && end.HasValue)
            {
                var query = baseQuery
                    .Where(x =>
                        x.PaymentDateTime >= start.Value &&
                        x.PaymentDateTime <= end.Value);

                response.Custom = await query
                    .GroupBy(x => x.PaymentDateTime.Date)
                    .Select(g => new RevenueGraphPoint
                    {
                        Label = g.Key.ToString("yyyy-MM-dd"),

                        Revenue =
                            g.Where(x => !x.IsGiftAid)
                            .Sum(x => (decimal?)x.Amount) ?? 0,

                        GiftAidRevenue =
                            g.Where(x => x.IsGiftAid)
                            .Sum(x => (decimal?)x.Amount) ?? 0
                    })
                    .OrderBy(x => x.Label)
                    .ToListAsync();

                return response;
            }

            // =====================
            // DASHBOARD MODE
            // =====================

            var today = DateTime.UtcNow.Date;

            var yearStart =
                new DateTime(today.Year, 1, 1);

            var sixWeeksAgo =
                today.AddDays(-42);

            // ---------------------
            // MONTHLY (Full Year)
            // ---------------------

            var monthlyData = await baseQuery
                .Where(x => x.PaymentDateTime >= yearStart)
                .GroupBy(x => x.PaymentDateTime.Month)
                .Select(g => new
                {
                    Month = g.Key,

                    Revenue =
                        g.Where(x => !x.IsGiftAid)
                        .Sum(x => (decimal?)x.Amount) ?? 0,

                    GiftAidRevenue =
                        g.Where(x => x.IsGiftAid)
                        .Sum(x => (decimal?)x.Amount) ?? 0
                })
                .ToListAsync();

            response.Monthly = monthlyData
                .Select(x => new RevenueGraphPoint
                {
                    Label = new DateTime(
                        today.Year,
                        x.Month,
                        1).ToString("MMM"),

                    Revenue = x.Revenue,
                    GiftAidRevenue = x.GiftAidRevenue
                })
                .OrderBy(x =>
                    DateTime.ParseExact(
                        x.Label,
                        "MMM",
                        CultureInfo.InvariantCulture).Month)
                .ToList();

            // ---------------------
            // WEEKLY (Last 6 Weeks)
            // ---------------------

            response.Weekly =
                await baseQuery
                .Where(x => x.PaymentDateTime >= sixWeeksAgo)
                .GroupBy(x =>
                    EF.Functions.DateDiffWeek(
                        sixWeeksAgo,
                        x.PaymentDateTime))
                .Select(g => new RevenueGraphPoint
                {
                    Label =
                        "Week " + (g.Key + 1),

                    Revenue =
                        g.Where(x => !x.IsGiftAid)
                        .Sum(x => (decimal?)x.Amount) ?? 0,

                    GiftAidRevenue =
                        g.Where(x => x.IsGiftAid)
                        .Sum(x => (decimal?)x.Amount) ?? 0
                })
                .OrderBy(x => x.Label)
                .ToListAsync();

            // ---------------------
            // TODAY (Hourly)
            // ---------------------

            var todayData = await baseQuery
                .Where(x => x.PaymentDateTime >= today)
                .GroupBy(x => x.PaymentDateTime.Hour)
                .Select(g => new
                {
                    Hour = g.Key,

                    Revenue =
                        g.Where(x => !x.IsGiftAid)
                        .Sum(x => (decimal?)x.Amount) ?? 0,

                    GiftAidRevenue =
                        g.Where(x => x.IsGiftAid)
                        .Sum(x => (decimal?)x.Amount) ?? 0
                })
                .ToListAsync();

            response.Today = todayData
                .Select(x => new RevenueGraphPoint
                {
                    Label = x.Hour + ":00",

                    Revenue = x.Revenue,
                    GiftAidRevenue = x.GiftAidRevenue
                })
                .OrderBy(x =>
                    int.Parse(x.Label.Replace(":00", "")))
                .ToList();

            return response;
        }


    }
}
