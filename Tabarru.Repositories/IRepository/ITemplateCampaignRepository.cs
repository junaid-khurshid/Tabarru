using Tabarru.Repositories.Models;

namespace Tabarru.Repositories.IRepository
{
    public interface ITemplateCampaignRepository
    {
        Task<bool> RemoveTemplateCampaignsRanges(ICollection<TemplateCampaign> templateCampaigns);
    }
}
