using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tabarru.Attributes;
using Tabarru.Services.Models;

namespace Tabarru.RequestModels
{
    public class TemplateCreateRequest
    {
        [Required]
        public string Name { get; set; }

        [ValidateFile(5, 2000)]
        public IFormFile Icon { get; set; }
        public string Message { get; set; }

        [Required]
        public string Modes { get; set; }

        // Helper to get deserialized list
        [NotMapped]
        public List<ModeCreateRequest> ParsedModes =>
            string.IsNullOrWhiteSpace(Modes)
                ? new List<ModeCreateRequest>()
                : JsonConvert.DeserializeObject<List<ModeCreateRequest>>(Modes);
    }

    static class TemplateExtension
    {
        public static TemplateDto MapToDto(this TemplateCreateRequest request, string CharityId)
        {
            return new TemplateDto
            {
                Name = request.Name,
                CharityId = CharityId,
                Modes = request.ParsedModes.Select(x => x.MapToDto()).ToList(),
                Icon = request.Icon,
                Message = request.Message,
            };
        }
    }
}
