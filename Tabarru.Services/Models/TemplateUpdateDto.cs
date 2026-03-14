using Microsoft.AspNetCore.Http;

namespace Tabarru.Services.Models
{
    public class TemplateUpdateDto
    {
        public string TemplateId { get; set; }
        public string Name { get; set; }
        public string CharityId { get; set; }
        public IFormFile Icon { get; set; }
        public string Message { get; set; }
        public int ShapeId { get; set; }
        public string SecondaryColor { get; set; }
        public string PrimaryColor { get; set; }
        public List<ModeDto> Modes { get; set; }
    }
}