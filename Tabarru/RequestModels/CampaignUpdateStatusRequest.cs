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
        public static CampaignUpdateStatusDto MapToDto(this CampaignUpdateStatusRequest campaignUpdateStatusRequest, string CharityId)
        {
            return new CampaignUpdateStatusDto
            {
                CampaignId = campaignUpdateStatusRequest.CampaignId,
                CharityId = CharityId,
                IsDefault = campaignUpdateStatusRequest.IsDefault,
                IsEnabled = campaignUpdateStatusRequest.IsEnabled,
            };
        }

    }
}
