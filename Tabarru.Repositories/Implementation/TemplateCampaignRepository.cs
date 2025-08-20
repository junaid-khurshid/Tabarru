using Tabarru.Repositories.DatabaseContext;
using Tabarru.Repositories.IRepository;
using Tabarru.Repositories.Models;

namespace Tabarru.Repositories.Implementation
{
    public class TemplateCampaignRepository : ITemplateCampaignRepository
    {
        private readonly DbStorageContext dbStorageContext;

        public TemplateCampaignRepository(DbStorageContext dbStorageContext)
        {
            this.dbStorageContext = dbStorageContext;
        }

        public async Task<bool> RemoveTemplateCampaignsRanges(ICollection<TemplateCampaign> templateCampaigns)
        {
            dbStorageContext.TemplateCampaigns.RemoveRange(templateCampaigns);
            return await dbStorageContext.SaveChangesAsync() > 0;
        }
    }
}
