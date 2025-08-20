namespace Tabarru.Services.Models
{
    public class TemplateUpdateDto
    {
        public string TemplateId { get; set; }

        public string CharityId { get; set; }

        public string Name { get; set; }

        public List<string> CampaignIds { get; set; }
    }
}
