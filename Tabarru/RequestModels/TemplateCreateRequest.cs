using Tabarru.Services.Models;

namespace Tabarru.RequestModels
{
    public class TemplateCreateRequest
    {
        public string Name { get; set; }
        public List<string> CampaignIds { get; set; } = new();
    }

    static class TemplateExtension
    {
        public static TemplateDto MapToDto(this TemplateCreateRequest request, string CharityId)
        {
            return new TemplateDto
            {
                Name = request.Name,
                CharityId = CharityId,
                CampaignIds = request.CampaignIds
            };
        }
    }
}
