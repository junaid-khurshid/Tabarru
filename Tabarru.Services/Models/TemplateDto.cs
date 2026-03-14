using Microsoft.AspNetCore.Http;
using Tabarru.Repositories.Models;

namespace Tabarru.Services.Models
{

    public class TemplateDto
    {
        public string Name { get; set; }
        public string CharityId { get; set; }
        public string CampaignId { get; set; }
        public IFormFile Icon { get; set; }
        public string Message { get; set; }
        public int ShapeId { get; set; }
        public string SecondaryColor { get; set; }
        public string PrimaryColor { get; set; }
        public List<ModeDto> Modes { get; set; }
    }

    static class TemplateExtension
    {
        public static TemplateReadDto MapToDto(this Template template)
        {
            return new TemplateReadDto
            {
                Id = template.Id,
                CharityId = template.CharityId,
                Name = template.Name,
                Icon = template.Icon,
                Message = template.Message,
                SecondaryColor = template.SecondaryColor,
                PrimaryColor = template.PrimaryColor,
                ShapeId = template.ShapeId ?? 0,
                Modes = template.Modes?.Where(m => !m.IsDeleted).Select(x => x.MapToDto()) ?? new List<ModeReadDto>()
            };
        }
    }

    public class TemplateReadDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Message { get; set; }
        public int? ShapeId { get; set; }
        public string? SecondaryColor { get; set; }
        public string PrimaryColor { get; set; }
        public string CharityId { get; set; }
        public IEnumerable<ModeReadDto> Modes { get; set; }
    }

}
