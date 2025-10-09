using Microsoft.EntityFrameworkCore;
using Tabarru.Repositories.DatabaseContext;
using Tabarru.Repositories.IRepository;
using Tabarru.Repositories.Models;

namespace Tabarru.Repositories.Implementation
{
    public class CampaignRepository : ICampaignRepository
    {
        private readonly DbStorageContext dbStorageContext;

        public CampaignRepository(DbStorageContext dbStorageContext)
        {
            this.dbStorageContext = dbStorageContext;
        }

        public async Task<bool> AddAsync(Campaign campaign)
        {
            dbStorageContext.Campaigns.Add(campaign);
            return await dbStorageContext.SaveChangesAsync() > 0;
        }

        public async Task<Campaign> GetAllByCharityIdAndDefaultOneOnlyAsync(string CharityId)
        {
            return await dbStorageContext.Campaigns.FirstOrDefaultAsync(x => x.CharityId.Equals(CharityId) && x.IsDefault == true);
        }

        public async Task<Campaign> GetByCampaignIdAndCharityIdOnlyAsync(string CampaignId, string CharityId)
        {
            return await dbStorageContext.Campaigns.FirstOrDefaultAsync(x => x.Id.Equals(CampaignId) & x.CharityId.Equals(CharityId));
        }

        public async Task<IEnumerable<Campaign>> GetAllByCharityIdAsync(string CharityId)
        {
            return await dbStorageContext.Campaigns.Where(x => x.CharityId.Equals(CharityId)).OrderByDescending(c => c.CreatedDate).ToListAsync();
        }

        public async Task<Campaign> GetByIdAsync(string Id)
            => await dbStorageContext.Campaigns.FirstOrDefaultAsync(c => c.Id == Id);

        public async Task<bool> AnyByIdAsync(string Id)
            => await dbStorageContext.Campaigns.AnyAsync(c => c.Id == Id);

        public async Task<Campaign> GetByNameAndCharityIdAsync(string Name, string CharityId)
            => await dbStorageContext.Campaigns.FirstOrDefaultAsync(c => c.Name == Name && c.CharityId.Equals(CharityId));

        public async Task<bool> UpdateAsync(Campaign campaign)
        {
            dbStorageContext.Campaigns.Attach(campaign);
            dbStorageContext.Entry(campaign).State = EntityState.Modified;
            return await dbStorageContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Campaign campaign)
        {
            dbStorageContext.Campaigns.Remove(campaign);
            return await dbStorageContext.SaveChangesAsync() > 0;
        }
    }
}
