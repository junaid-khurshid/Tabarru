using Microsoft.EntityFrameworkCore;
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
    }
}
