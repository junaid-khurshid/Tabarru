using Microsoft.EntityFrameworkCore;
using Tabarru.Common.Models;
using Tabarru.Repositories.DatabaseContext;
using Tabarru.Repositories.IRepository;

namespace Tabarru.Repositories.Implementation
{
    public class DonationRepository : IDonationRepository
    {
        private readonly DbStorageContext dbStorageContext;

        public DonationRepository(DbStorageContext dbStorageContext)
        {
            this.dbStorageContext = dbStorageContext;
        }

        public async Task<List<DonationReportResponse>> GetAllTransactionsAsync(string charityId)
        {
            return await dbStorageContext.PaymentDetails
                .Where(p => p.CharityId == charityId && !p.IsDeleted)
                .GroupJoin(
                    dbStorageContext.GiftAidDetails.Where(g => !g.IsDeleted),
                    p => p.Id,
                    g => g.PaymentDetailId,
                    (p, g) => new { Payment = p, GiftAid = g.DefaultIfEmpty() }
                )
                .SelectMany(
                    x => x.GiftAid,
                    (x, g) => new DonationReportResponse
                    {
                        DonorId = x.Payment.CustomerId,
                        LatestDonationDate = x.Payment.PaymentDateTime,
                        GiftAid = x.Payment.IsGiftAid,
                        Title = g != null ? g.Title : null,
                        FirstName = g != null ? g.FirstName : null,
                        Address = g != null ? g.Address : null,
                        Postcode = g != null ? g.Postcode : null,
                        Donation = x.Payment.Amount
                    }
                )
                .ToListAsync();
        }

        public async Task<List<DonationReportResponse>> GetGiftAidTransactionsAsync(string charityId)
        {
            return await dbStorageContext.PaymentDetails
                .Where(p => p.CharityId == charityId
                         && !p.IsDeleted
                         && p.IsGiftAid)
                .GroupJoin(
                    dbStorageContext.GiftAidDetails.Where(g => !g.IsDeleted),
                    p => p.Id,
                    g => g.PaymentDetailId,
                    (p, g) => new { Payment = p, GiftAid = g.DefaultIfEmpty() }
                )
                .SelectMany(
                    x => x.GiftAid,
                    (x, g) => new DonationReportResponse
                    {
                        DonorId = x.Payment.CustomerId,
                        LatestDonationDate = x.Payment.PaymentDateTime,
                        GiftAid = x.Payment.IsGiftAid,
                        Title = g != null ? g.Title : null,
                        FirstName = g != null ? g.FirstName : null,
                        Address = g != null ? g.Address : null,
                        Postcode = g != null ? g.Postcode : null,
                        Donation = x.Payment.Amount
                    }
                )
                .ToListAsync();
        }

        public async Task<List<DonationReportResponse>> GetTransactionsWithoutGiftAidAsync(string charityId)
        {
            return await dbStorageContext.PaymentDetails
                .Where(p => p.CharityId == charityId && !p.IsDeleted && !p.IsGiftAid)
                .GroupJoin(
                    dbStorageContext.GiftAidDetails.Where(g => !g.IsDeleted),
                    p => p.Id,
                    g => g.PaymentDetailId,
                    (p, g) => new { Payment = p, GiftAid = g.DefaultIfEmpty() }
                )
                .SelectMany(
                    x => x.GiftAid,
                    (x, g) => new DonationReportResponse
                    {
                        DonorId = x.Payment.CustomerId,
                        LatestDonationDate = x.Payment.PaymentDateTime,
                        Title = g != null ? g.Title : null,
                        FirstName = g != null ? g.FirstName : null,
                        Address = g != null ? g.Address : null,
                        Postcode = g != null ? g.Postcode : null,
                        Donation = x.Payment.Amount,
                        GiftAid = false
                    }
                )
                .ToListAsync();
        }
    }
}
