using Tabarru.Attributes;
using Tabarru.Services.Models;

namespace Tabarru.RequestModels
{
    public class TemplateCreateRequest
    {
        public string Name { get; set; }
        [ValidateFile(5, 2000)]
        public IFormFile Icon { get; set; }
        public string Message { get; set; }
        public List<ModeCreateRequest> Modes { get; set; } = new();
    }

    static class TemplateExtension
    {
        public static TemplateDto MapToDto(this TemplateCreateRequest request, string CharityId)
        {
            return new TemplateDto
            {
                Name = request.Name,
                CharityId = CharityId,
                Modes = request.Modes.Select(x => x.MapToDto()).ToList(),
                Icon = request.Icon,
                Message = request.Message
            };
        }
    }
}
