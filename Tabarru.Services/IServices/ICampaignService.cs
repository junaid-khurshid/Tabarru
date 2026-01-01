using Tabarru.Common.Models;
using Tabarru.Services.Models;

namespace Tabarru.Services.IServices
{
    public interface ICampaignService
    {
        Task<Response> CreateAsync(CampaignDto dto);

        Task<Response> UpdateAsync(CampaignDto dto);

        Task<Response<IList<CampaignReadDto>>> GetAllByCharityIDAsync(string CharityId);

        Task<Response<CampaignReadDto>> GetByIdAsync(string id);

        Task<Response> UpdateStatusAsync(CampaignUpdateStatusDto dto);

        Task<Response> DeleteCampaignAsync(string id);
    }
}