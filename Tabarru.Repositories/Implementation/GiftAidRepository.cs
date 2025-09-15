using Microsoft.EntityFrameworkCore;
using Tabarru.Repositories.DatabaseContext;
using Tabarru.Repositories.IRepository;
using Tabarru.Repositories.Models;

namespace Tabarru.Repositories.Implementation
{
    public class GiftAidRepository : IGiftAidRepository
    {
        private readonly DbStorageContext dbStorageContext;

        public GiftAidRepository(DbStorageContext dbStorageContext)
        {
            this.dbStorageContext = dbStorageContext;
        }
        public async Task<bool> AddAsync(GiftAidDetail giftAidDetail)
        {
            dbStorageContext.GiftAidDetails.Add(giftAidDetail);
            return await dbStorageContext.SaveChangesAsync() > 0;
        }

        public async Task<GiftAidDetail> GetByIdAsync(string PaymentId)
        {
            return await dbStorageContext.GiftAidDetails.FirstOrDefaultAsync(x => x.Id.Equals(PaymentId));
        }

        public async Task<bool> UpdateAsync(GiftAidDetail giftAidDetail)
        {
            dbStorageContext.GiftAidDetails.Update(giftAidDetail);
            return await dbStorageContext.SaveChangesAsync() > 0;
        }
    }
}
