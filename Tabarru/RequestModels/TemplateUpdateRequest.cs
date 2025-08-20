using System.ComponentModel.DataAnnotations;
using Tabarru.Services.Models;

namespace Tabarru.RequestModels
{
    public class TemplateUpdateRequest
    {
        [Required]
        public string TemplateId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public List<string> CampaignIds { get; set; }
    }

    static class TemplateUpdateExtension
    {
        public static TemplateUpdateDto MaptoDto(this TemplateUpdateRequest templateUpdateRequest, string CharityId)
        {
            return new TemplateUpdateDto
            {
                TemplateId = templateUpdateRequest.TemplateId,
                CharityId = CharityId,
                CampaignIds = templateUpdateRequest.CampaignIds,
                Name = templateUpdateRequest.Name,
            };
        }
    }
}
