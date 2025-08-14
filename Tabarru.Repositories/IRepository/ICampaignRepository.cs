using Tabarru.Repositories.Models;

namespace Tabarru.Repositories.IRepository
{
    public interface ICampaignRepository
    {
        Task<bool> AddAsync(Campaign campaign);
        Task<Campaign> GetByIdAsync(string Id);
        Task<IEnumerable<Campaign>> GetAllByCharityIdAsync(string CharityId);
        Task<Campaign> GetAllByCharityIdAndDefaultOneOnlyAsync(string CharityId);
        Task<Campaign> GetByNameAndCharityIdAsync(string Name, string CharityId);
        Task<bool> UpdateAsync(Campaign campaign);
        Task<Campaign> GetAllByCampaignIdAndCharityIdOnlyAsync(string CampaignId, string CharityId);
    }
}
