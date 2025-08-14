using System.ComponentModel.DataAnnotations;
using Tabarru.Services.Models;

namespace Tabarru.RequestModels
{
    public class CampaignUpdateStatusRequest
    {
        [Required]
        public string CampaignId { get; set; }

        [Required]
        public bool IsEnabled { get; set; }

        [Required]
        public bool IsDefault { get; set; }
    }

    static class CampaignUpdateStatusExtension
    {
        public static CampaignUpdateStatusDto MapToDto(this CampaignUpdateStatusRequest loginRequest, string CharityId)
        {
            return new CampaignUpdateStatusDto
            {
                CampaignId = loginRequest.CampaignId,
                CharityId = CharityId,
                IsDefault = loginRequest.IsDefault,
                IsEnabled = loginRequest.IsEnabled,
            };
        }

    }
}
