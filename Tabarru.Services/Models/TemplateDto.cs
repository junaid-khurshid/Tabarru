using Tabarru.Repositories.Models;

namespace Tabarru.Services.Models
{

    public class TemplateDto
    {
        public string Name { get; set; }
        public string CharityId { get; set; }
        public string CampaignId { get; set; }
        public string Icon { get; set; }
        public string Message { get; set; }
        public List<ModeDto> Modes { get; set; } = new();
    }

    static class TemplateExtension
    {
        public static TemplateReadDto MapToDto(this Template campaign)
        {
            return new TemplateReadDto
            {
                Id = campaign.Id,
                CharityId = campaign.CharityId,
                Name = campaign.Name,
                Icon = campaign.Icon,
                CampaignId = campaign.CampaignId,
                Message = campaign.Message,
                Modes = campaign.Modes.Select(x => x.MapToDto())
            };
        }
    }

    public class TemplateReadDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Message { get; set; }
        public string CharityId { get; set; }
        public string CampaignId { get; set; }
        public IEnumerable<ModeReadDto> Modes { get; set; }
    }

}
