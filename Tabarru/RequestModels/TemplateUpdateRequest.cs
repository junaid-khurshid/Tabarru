using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tabarru.Attributes;
using Tabarru.Services.Models;

namespace Tabarru.RequestModels
{
    public class TemplateUpdateRequest
    {
        [Required]
        public string TemplateId { get; set; }

        [Required]
        public string Name { get; set; }

        [ValidateFile(5, 2000)]
        public IFormFile Icon { get; set; }
        public string Message { get; set; }

        [Required]
        public string Modes { get; set; }

        [NotMapped]
        public List<ModeCreateRequest> ParsedModes =>
            string.IsNullOrWhiteSpace(Modes)
                ? new List<ModeCreateRequest>()
                : JsonConvert.DeserializeObject<List<ModeCreateRequest>>(Modes);
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
                Message = templateUpdateRequest.Message,
                Icon = templateUpdateRequest.Icon,
                Modes = templateUpdateRequest.ParsedModes.Select(x => x.MapToDto()).ToList()
            };
        }
    }
}
