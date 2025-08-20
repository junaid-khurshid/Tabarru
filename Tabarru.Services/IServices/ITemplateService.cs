using Tabarru.Common.Models;
using Tabarru.Services.Models;

namespace Tabarru.Services.IServices
{
    public interface ITemplateService
    {
        Task<Response<IEnumerable<TemplateReadDto>>> GetAllTemplatesAsync(string charityId);
        Task<Response<TemplateReadDto>> GetTemplateByIdAsync(string id);
        Task<Response> CreateTemplateAsync(TemplateDto request);
        Task<Response> UpdateTemplateAsync(TemplateUpdateDto request);
        Task<Response> DeleteTemplateAsync(string id);
    }
}
