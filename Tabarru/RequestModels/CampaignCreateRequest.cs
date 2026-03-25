using System.ComponentModel.DataAnnotations;
using Tabarru.Attributes;
using Tabarru.Services.Models;

namespace Tabarru.RequestModels
{
    public class CampaignCreateRequest
    {
        [Required(ErrorMessage = "REQUIRED")]
        public string Name { get; set; }

        [Required(ErrorMessage = "REQUIRED")]
        //[ValidateFile(5, 2000)]
        public IFormFile Icon { get; set; }

        [Required(ErrorMessage = "REQUIRED")]
        public string ListOfAmounts { get; set; }

        public bool isMembershipForm { get; set; } = false;
        public bool isStudentForm { get; set; } = false;
    }

    static class CampaignExtension
    {
        public static CampaignDto MapToDto(this CampaignCreateRequest request, string CharityId)
        {
            return new CampaignDto
            {
                Name = request.Name,
                Icon = request.Icon,
                isStudentForm = request.isStudentForm,
                isMembershipForm = request.isMembershipForm,
                CharityId = CharityId,
                ListOfAmounts = request.ListOfAmounts,
            };
        }
    }
}
