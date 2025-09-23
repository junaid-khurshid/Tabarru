using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tabarru.Common.Helper;
using Tabarru.Common.Models;
using Tabarru.RequestModels;
using Tabarru.Services.IServices;
using Tabarru.Services.Models;

namespace Tabarru.Controllers
{
    [Authorize(Policy = "KycApprovedOnly", Roles = "USER,ADMIN")]
    [Route("api/[controller]")]
    [ApiController]
    public class TemplatesController : ControllerBase
    {
        private readonly ITemplateService templateService;

        public TemplatesController(ITemplateService templateService)
        {
            this.templateService = templateService;
        }

        [HttpGet("charity/{charityId}")]
        public async Task<Response<IEnumerable<TemplateReadDto>>> GetAll(string charityId)
        {
            return await templateService.GetAllTemplatesAsync(charityId);
        }

        [HttpGet("{id}")]
        public async Task<Response<TemplateReadDto>> GetById(string id)
        {
            return await templateService.GetTemplateByIdAsync(id);
        }

        [HttpPost]
        public async Task<Response> Create([FromBody] TemplateCreateRequest request)
        {
            return await templateService.CreateTemplateAsync(request.MapToDto(TokenClaimHelper.GetId(User)));
        }

        [HttpPut]
        public async Task<Response> Update([FromBody] TemplateUpdateRequest request)
        {
            return await templateService.UpdateTemplateAsync(request.MaptoDto(TokenClaimHelper.GetId(User)));
        }

        [HttpDelete("{id}")]
        public async Task<Response> Delete(string id)
        {
            return await templateService.DeleteTemplateAsync(id);

        }
    }
}
