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
        public string Icon { get; set; }
        public string Message { get; set; }
        [Required]
        public string CampaignId { get; set; }
        public List<ModeCreateRequest> Modes { get; set; }
    }

    static class TemplateUpdateExtension
    {
        public static TemplateUpdateDto MaptoDto(this TemplateUpdateRequest templateUpdateRequest, string CharityId)
        {
            return new TemplateUpdateDto
            {
                TemplateId = templateUpdateRequest.TemplateId,
                CharityId = CharityId,
                Name = templateUpdateRequest.Name,
                CampaignId = templateUpdateRequest.CampaignId,
                Message = templateUpdateRequest.Message,
                Icon = templateUpdateRequest.Icon,
                Modes = templateUpdateRequest.Modes.Select(x => x.MapToDto()).ToList()
            };
        }
    }
}
