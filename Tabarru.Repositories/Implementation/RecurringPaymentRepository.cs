using Microsoft.EntityFrameworkCore;
using Tabarru.Repositories.DatabaseContext;
using Tabarru.Repositories.IRepository;
using Tabarru.Repositories.Models;

namespace Tabarru.Repositories.Implementation
{
    public class RecurringPaymentRepository : IRecurringPaymentRepository
    {
        private readonly DbStorageContext dbStorageContext;

        public RecurringPaymentRepository(DbStorageContext dbStorageContext)
        {
            this.dbStorageContext = dbStorageContext;
        }
        public async Task<bool> AddAsync(RecurringPaymentDetail recurringPaymentDetail)
        {
            dbStorageContext.RecurringPaymentDetails.Add(recurringPaymentDetail);
            return await dbStorageContext.SaveChangesAsync() > 0;
        }

        public async Task<RecurringPaymentDetail> GetByIdAsync(string Id)
        {
            return await dbStorageContext.RecurringPaymentDetails.FirstOrDefaultAsync(x => x.Id.Equals(Id));
        }

        public async Task<bool> UpdateAsync(RecurringPaymentDetail recurringPaymentDetail)
        {
            dbStorageContext.RecurringPaymentDetails.Update(recurringPaymentDetail);
            return await dbStorageContext.SaveChangesAsync() > 0;
        }
    }
}
