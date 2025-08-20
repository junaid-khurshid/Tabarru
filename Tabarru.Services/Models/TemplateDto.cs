namespace Tabarru.Services.Models
{

    public class TemplateDto
    {
        public string CharityId { get; set; }
        public string Name { get; set; }
        public List<string> CampaignIds { get; set; }
    }

    public class TemplateReadDto
    {
        public string Id { get; set; }

        public string CharityId { get; set; }

        public string Name { get; set; }

        public List<string> Campaigns { get; set; }
    }
}
