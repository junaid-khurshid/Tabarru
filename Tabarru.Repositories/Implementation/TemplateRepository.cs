using Microsoft.EntityFrameworkCore;
using Tabarru.Repositories.DatabaseContext;
using Tabarru.Repositories.IRepository;
using Tabarru.Repositories.Models;

namespace Tabarru.Repositories.Implementation
{
    public class TemplateRepository : ITemplateRepository
    {
        private readonly DbStorageContext dbStorageContext;

        public TemplateRepository(DbStorageContext dbStorageContext)
        {
            this.dbStorageContext = dbStorageContext;
        }

        public async Task<Template> GetByIdAsync(string id)
        {
            return await dbStorageContext.Templates
            .Include(t => t.Modes)
            .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Template>> GetAllTemplatesByCharityIdAsync(string CharityId) =>
            await dbStorageContext.Templates.Where(x => x.CharityId.Equals(CharityId))
                .Include(t => t.Modes)
                    .ThenInclude(c => c.Campaign)
                .ToListAsync();

        public async Task<bool> AddAsync(Template template)
        {
            await dbStorageContext.Templates.AddAsync(template);
            return await dbStorageContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(Template template)
        {
            dbStorageContext.Templates.Update(template);
            return await dbStorageContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Template template)
        {
            dbStorageContext.Templates.Remove(template);
            return await dbStorageContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> ExistsWithCampaignAsync(string campaignId) =>
        await dbStorageContext.Modes.AnyAsync(m => m.CampaignId == campaignId);
    }
}
