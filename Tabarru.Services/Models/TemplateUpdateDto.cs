using Microsoft.AspNetCore.Http;

namespace Tabarru.Services.Models
{
    public class TemplateUpdateDto
    {
        public string TemplateId { get; set; }
        public string Name { get; set; }
        public string CharityId { get; set; }
        public string CampaignId { get; set; }
        public IFormFile Icon { get; set; }
        public string Message { get; set; }
        public List<ModeDto> Modes { get; set; }
    }
}