using System.ComponentModel.DataAnnotations;
using Tabarru.Attributes;
using Tabarru.Services.Models;

namespace Tabarru.RequestModels
{
    public class CampaignUpdateRequest
    {
        [Required(ErrorMessage = "REQUIRED")]
        public string CampaignId { get; set; }
        [Required(ErrorMessage = "REQUIRED")]
        public string Name { get; set; }

        [Required(ErrorMessage = "REQUIRED")]
        [ValidateFile(5, 2000)]
        public IFormFile Icon { get; set; }

        [Required(ErrorMessage = "REQUIRED")]
        public string ListOfAmounts { get; set; }

        public bool IsEnabled { get; set; } = true;
        public bool IsDefault { get; set; } = false;
    }

    static class CampaignUpdateExtension
    {
        public static CampaignDto MapToDto(this CampaignUpdateRequest request, string CharityId)
        {
            return new CampaignDto
            {
                Name = request.Name,
                Icon = request.Icon,
                IsEnabled = request.IsEnabled,
                IsDefault = request.IsDefault,
                CharityId = CharityId,
                ListOfAmounts = request.ListOfAmounts,
                Id = request.CampaignId
            };
        }
    }
}
