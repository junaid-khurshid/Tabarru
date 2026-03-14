using System.ComponentModel.DataAnnotations;
using Tabarru.Services.Models;

namespace Tabarru.RequestModels
{
    public class CampaignUpdateStatusRequest
    {
        [Required]
        public string CampaignId { get; set; }

        [Required]
        public bool isMembershipForm { get; set; }

        [Required]
        public bool isStudentForm { get; set; }
    }

    static class CampaignUpdateStatusExtension
    {
        public static CampaignUpdateStatusDto MapToDto(this CampaignUpdateStatusRequest campaignUpdateStatusRequest, string CharityId)
        {
            return new CampaignUpdateStatusDto
            {
                CampaignId = campaignUpdateStatusRequest.CampaignId,
                CharityId = CharityId,
                isStudentForm = campaignUpdateStatusRequest.isStudentForm,
                isMembershipForm = campaignUpdateStatusRequest.isMembershipForm,
            };
        }

    }
}
