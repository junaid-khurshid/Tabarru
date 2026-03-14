using Microsoft.EntityFrameworkCore;
using Tabarru.Common.Models;
using Tabarru.Repositories.DatabaseContext;
using Tabarru.Repositories.IRepository;
using Tabarru.Repositories.Models;

namespace Tabarru.Repositories.Implementation
{
    public class DonationRepository : IDonationRepository
    {
        private readonly DbStorageContext dbStorageContext;

        public DonationRepository(DbStorageContext dbStorageContext)
        {
            this.dbStorageContext = dbStorageContext;
        }

        public async Task<PagedResponse<DonationReportResponse>> GetAllTransactionsAsync(string charityId, int pageNumber, int pageSize)
        {
            pageSize = pageSize > 100 ? 100 : pageSize;

            var query = dbStorageContext.PaymentDetails
                .Where(p => p.CharityId == charityId && !p.IsDeleted)
                .OrderByDescending(x => x.PaymentDateTime)
                .GroupJoin(dbStorageContext.GiftAidDetails.Where(g => !g.IsDeleted), p => p.Id, g => g.PaymentDetailId, (p, g) => new { Payment = p, GiftAid = g.DefaultIfEmpty() })
                .SelectMany(x => x.GiftAid, (x, g) => new DonationReportResponse { DonorId = x.Payment.CustomerId, LatestDonationDate = x.Payment.PaymentDateTime, GiftAid = x.Payment.IsGiftAid, Title = g != null ? g.Title : null, FirstName = g != null ? g.FirstName : null, Address = g != null ? g.Address : null, Postcode = g != null ? g.Postcode : null, Donation = x.Payment.Amount });

            var totalRecords = await query.CountAsync();

            var data = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedResponse<DonationReportResponse> { PageNumber = pageNumber, PageSize = pageSize, TotalRecords = totalRecords, Data = data };
        }

        public async Task<List<PaymentDetailResponse>> GetTodayTransactionsAsync(string charityId)
        {
            var today = DateTime.UtcNow.Date;
            var tomorrow = today.AddDays(1);

            var data = await dbStorageContext.PaymentDetails.AsNoTracking()
                .Where(p =>
                    p.CharityId == charityId &&
                    !p.IsDeleted &&
                    p.PaymentDateTime >= today &&
                    p.PaymentDateTime < tomorrow)
                .OrderByDescending(p => p.PaymentDateTime)
                .Select(p => new PaymentDetailResponse
                {
                    Id = p.Id,
                    CharityId = p.CharityId,
                    TransactionId = p.TransactionId,
                    Status = p.Status,
                    Amount = p.Amount,
                    Currency = p.Currency,
                    VendorType = p.VendorType,
                    PaymentDateTime = p.PaymentDateTime,
                    Description = p.Description,
                    IsGiftAid = p.IsGiftAid,
                    IsBankFeeCovered = p.IsBankFeeCovered,
                    IsRecurringPayment = p.IsRecurringPayment
                })
                .ToListAsync();

            return data;
        }

        public async Task<PagedResponse<DonationReportResponse>> GetGiftAidTransactionsAsync(string charityId, int pageNumber, int pageSize)
        {
            pageSize = pageSize > 100 ? 100 : pageSize;

            var query = dbStorageContext.PaymentDetails
                .Where(p => p.CharityId == charityId && !p.IsDeleted && p.IsGiftAid)
                .OrderByDescending(x => x.PaymentDateTime)
                .GroupJoin(dbStorageContext.GiftAidDetails.Where(g => !g.IsDeleted), p => p.Id, g => g.PaymentDetailId, (p, g) => new { Payment = p, GiftAid = g.DefaultIfEmpty() })
                .SelectMany(x => x.GiftAid, (x, g) => new DonationReportResponse { DonorId = x.Payment.CustomerId, LatestDonationDate = x.Payment.PaymentDateTime, GiftAid = x.Payment.IsGiftAid, Title = g != null ? g.Title : null, FirstName = g != null ? g.FirstName : null, Address = g != null ? g.Address : null, Postcode = g != null ? g.Postcode : null, Donation = x.Payment.Amount });

            var totalRecords = await query.CountAsync();

            var data = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedResponse<DonationReportResponse> { PageNumber = pageNumber, PageSize = pageSize, TotalRecords = totalRecords, Data = data };
        }

        public async Task<PagedResponse<DonationReportResponse>> GetTransactionsWithoutGiftAidAsync(string charityId, int pageNumber, int pageSize)
        {
            pageSize = pageSize > 100 ? 100 : pageSize;

            var query = dbStorageContext.PaymentDetails
                .Where(p => p.CharityId == charityId && !p.IsDeleted && !p.IsGiftAid)
                .OrderByDescending(x => x.PaymentDateTime)
                .GroupJoin(dbStorageContext.GiftAidDetails.Where(g => !g.IsDeleted), p => p.Id, g => g.PaymentDetailId, (p, g) => new { Payment = p, GiftAid = g.DefaultIfEmpty() })
                .SelectMany(x => x.GiftAid, (x, g) => new DonationReportResponse { DonorId = x.Payment.CustomerId, LatestDonationDate = x.Payment.PaymentDateTime, GiftAid = false, Title = g != null ? g.Title : null, FirstName = g != null ? g.FirstName : null, Address = g != null ? g.Address : null, Postcode = g != null ? g.Postcode : null, Donation = x.Payment.Amount });

            var totalRecords = await query.CountAsync();

            var data = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedResponse<DonationReportResponse> { PageNumber = pageNumber, PageSize = pageSize, TotalRecords = totalRecords, Data = data };
        }
    }
}
