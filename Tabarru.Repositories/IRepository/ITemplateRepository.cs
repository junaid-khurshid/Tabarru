using Tabarru.Repositories.Models;

namespace Tabarru.Repositories.IRepository
{
    public interface ITemplateRepository
    {
        Task<Template> GetByIdAsync(string id);
        Task<IEnumerable<Template>> GetAllTemplatesByCharityIdAsync(string TemplateId);
        Task<bool> AddAsync(Template template);
        Task<bool> UpdateAsync(Template template);
        Task<bool> DeleteAsync(Template template);
        Task<bool> ExistsWithCampaignAsync(string campaignId);
    }
}
