using System.ComponentModel.DataAnnotations;
using Tabarru.Attributes;
using Tabarru.Services.Models;

namespace Tabarru.RequestModels
{
    public class CampaignCreateRequest
    {
        [Required(ErrorMessage = "REQUIRED")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "REQUIRED")]
        [ValidateFile(200, 2000)]
        public IFormFile Icon { get; set; }

        [Required(ErrorMessage = "REQUIRED")]
        public string ListOfAmounts { get; set; } = null!;

        public bool IsEnabled { get; set; } = true;
        public bool IsDefault { get; set; } = false;
    }

    static class CampaignExtension
    {
        public static CampaignDto MapToDto(this CampaignCreateRequest request, string CharityId)
        {
            return new CampaignDto
            {
                Name = request.Name,
                Icon = request.Icon,
                IsEnabled = request.IsEnabled,
                IsDefault = request.IsDefault,
                CharityId = CharityId,
                ListOfAmounts =  request.ListOfAmounts,
            };
        }
    }
}
