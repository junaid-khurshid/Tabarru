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

        public async Task<PagedResponse<DonationReportResponse>> GetAllTransactionsAsync(DateTime? start, DateTime? end, string charityId, int pageNumber, int pageSize)
        {
            pageSize = pageSize > 100 ? 100 : pageSize;

            var baseQuery = dbStorageContext.PaymentDetails
                .AsNoTracking()
                .Where(p => p.CharityId == charityId && !p.IsDeleted);

            if (start.HasValue && end.HasValue)
            {
                baseQuery = baseQuery.Where(x =>
                    x.PaymentDateTime >= start.Value &&
                    x.PaymentDateTime <= end.Value);
            }

            var query = baseQuery
                .OrderByDescending(x => x.PaymentDateTime)
                .GroupJoin(
                    dbStorageContext.GiftAidDetails.Where(g => !g.IsDeleted),
                    p => p.Id,
                    g => g.PaymentDetailId,
                    (p, g) => new { Payment = p, GiftAid = g.DefaultIfEmpty() })
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
                    });

            var totalRecords = await query.CountAsync();

            var data = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResponse<DonationReportResponse>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                Data = data
            };
        }

        public async Task<PagedResponse<DonationReportResponse>> GetGiftAidTransactionsAsync(DateTime? start, DateTime? end, string charityId, int pageNumber, int pageSize)
        {
            pageSize = pageSize > 100 ? 100 : pageSize;

            var baseQuery = dbStorageContext.PaymentDetails
                .AsNoTracking()
                .Where(p => p.CharityId == charityId && !p.IsDeleted && p.IsGiftAid);

            if (start.HasValue && end.HasValue)
            {
                baseQuery = baseQuery.Where(x =>
                    x.PaymentDateTime >= start.Value &&
                    x.PaymentDateTime <= end.Value);
            }

            var query = baseQuery
                .OrderByDescending(x => x.PaymentDateTime)
                .GroupJoin(
                    dbStorageContext.GiftAidDetails.Where(g => !g.IsDeleted),
                    p => p.Id,
                    g => g.PaymentDetailId,
                    (p, g) => new { Payment = p, GiftAid = g.DefaultIfEmpty() })
                .SelectMany(
                    x => x.GiftAid,
                    (x, g) => new DonationReportResponse
                    {
                        DonorId = x.Payment.CustomerId,
                        LatestDonationDate = x.Payment.PaymentDateTime,
                        GiftAid = true,
                        Title = g != null ? g.Title : null,
                        FirstName = g != null ? g.FirstName : null,
                        Address = g != null ? g.Address : null,
                        Postcode = g != null ? g.Postcode : null,
                        Donation = x.Payment.Amount
                    });

            var totalRecords = await query.CountAsync();

            var data = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResponse<DonationReportResponse>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                Data = data
            };
        }

        public async Task<PagedResponse<DonationReportResponse>> GetTransactionsWithoutGiftAidAsync(DateTime? start, DateTime? end, string charityId, int pageNumber, int pageSize)
        {
            pageSize = pageSize > 100 ? 100 : pageSize;

            var baseQuery = dbStorageContext.PaymentDetails
                .AsNoTracking()
                .Where(p => p.CharityId == charityId && !p.IsDeleted && !p.IsGiftAid);

            if (start.HasValue && end.HasValue)
            {
                baseQuery = baseQuery.Where(x =>
                    x.PaymentDateTime >= start.Value &&
                    x.PaymentDateTime <= end.Value);
            }

            var query = baseQuery
                .OrderByDescending(x => x.PaymentDateTime)
                .GroupJoin(
                    dbStorageContext.GiftAidDetails.Where(g => !g.IsDeleted),
                    p => p.Id,
                    g => g.PaymentDetailId,
                    (p, g) => new { Payment = p, GiftAid = g.DefaultIfEmpty() })
                .SelectMany(
                    x => x.GiftAid,
                    (x, g) => new DonationReportResponse
                    {
                        DonorId = x.Payment.CustomerId,
                        LatestDonationDate = x.Payment.PaymentDateTime,
                        GiftAid = false,
                        Title = g != null ? g.Title : null,
                        FirstName = g != null ? g.FirstName : null,
                        Address = g != null ? g.Address : null,
                        Postcode = g != null ? g.Postcode : null,
                        Donation = x.Payment.Amount
                    });

            var totalRecords = await query.CountAsync();

            var data = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResponse<DonationReportResponse>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                Data = data
            };
        }

        public async Task<PagedResponse<MembershipTransactionResponse>> GetMembershipTransactionsAsync(DateTime? start, DateTime? end, string charityId, int pageNumber, int pageSize)
        {
            pageSize = pageSize > 100 ? 100 : pageSize;

            var baseQuery = dbStorageContext.PaymentDetails
                .AsNoTracking()
                .Where(p => p.CharityId == charityId && !p.IsDeleted && p.isMembershipForm);

            if (start.HasValue && end.HasValue)
            {
                baseQuery = baseQuery.Where(x =>
                    x.PaymentDateTime >= start.Value &&
                    x.PaymentDateTime <= end.Value);
            }

            var query = baseQuery
                .OrderByDescending(x => x.PaymentDateTime)
                .GroupJoin(
                    dbStorageContext.MembershipDetails.Where(m => !m.IsDeleted),
                    p => p.Id,
                    m => m.PaymentDetailId,
                    (p, m) => new { Payment = p, Membership = m.DefaultIfEmpty() })
                .SelectMany(
                    x => x.Membership,
                    (x, m) => new MembershipTransactionResponse
                    {
                        // Payment
                        PaymentId = x.Payment.Id,
                        DonorId = x.Payment.CustomerId,
                        PaymentDateTime = x.Payment.PaymentDateTime,
                        Donation = x.Payment.Amount,

                        // Membership
                        FirstName = m != null ? m.FirstName : null,
                        LastName = m != null ? m.LastName : null,
                        HouseNumberAndName = m != null ? m.HouseNumberAndName : null,
                        PostalCode = m != null ? m.PostalCode : null,

                        // Descriptions
                        Description1 = m != null ? m.Description1 : null,
                        Description2 = m != null ? m.Description2 : null,
                        Description3 = m != null ? m.Description3 : null
                    });

            var totalRecords = await query.CountAsync();

            var data = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResponse<MembershipTransactionResponse>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                Data = data
            };
        }

        public async Task<PagedResponse<StudentFormTransactionResponse>> GetStudentFormTransactionsAsync(DateTime? start, DateTime? end, string charityId, int pageNumber, int pageSize)
        {
            pageSize = pageSize > 100 ? 100 : pageSize;

            var baseQuery = dbStorageContext.PaymentDetails
                .AsNoTracking()
                .Where(p => p.CharityId == charityId && !p.IsDeleted && p.isStudentForm);

            if (start.HasValue && end.HasValue)
            {
                baseQuery = baseQuery.Where(x =>
                    x.PaymentDateTime >= start.Value &&
                    x.PaymentDateTime <= end.Value);
            }

            var query = baseQuery
                .OrderByDescending(x => x.PaymentDateTime)
                .GroupJoin(
                    dbStorageContext.StudentFormDetails.Where(s => !s.IsDeleted),
                    p => p.Id,
                    s => s.PaymentDetailId,
                    (p, s) => new { Payment = p, Student = s.DefaultIfEmpty() })
                .SelectMany(
                    x => x.Student,
                    (x, s) => new StudentFormTransactionResponse
                    {
                        // Payment
                        PaymentId = x.Payment.Id,
                        DonorId = x.Payment.CustomerId,
                        PaymentDateTime = x.Payment.PaymentDateTime,
                        Donation = x.Payment.Amount,

                        // Student Form
                        StudentName = s != null ? s.StudentName : null,
                        ParentName = s != null ? s.ParentName : null,
                        FullAddress = s != null ? s.FullAddress : null,
                        StudentId = s != null ? s.StudentId : null,
                        ParentId = s != null ? s.ParentId : null,
                        StudentAmount = s != null ? s.Amount : null,
                        Period = s != null ? s.Period : null,
                        Notes = s != null ? s.Notes : null,
                        Description1 = s != null ? s.Description1 : null,
                        Description2 = s != null ? s.Description2 : null,
                        Description3 = s != null ? s.Description3 : null
                    });

            var totalRecords = await query.CountAsync();

            var data = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResponse<StudentFormTransactionResponse>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                Data = data
            };
        }
    }
}
