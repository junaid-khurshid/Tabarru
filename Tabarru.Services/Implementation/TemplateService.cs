using System.Net;
using Tabarru.Common.Enums;
using Tabarru.Common.Models;
using Tabarru.Repositories.IRepository;
using Tabarru.Repositories.Models;
using Tabarru.Services.IServices;
using Tabarru.Services.Models;

namespace Tabarru.Services.Implementation
{
    public class TemplateService : ITemplateService
    {
        private readonly ITemplateRepository templateRepository;
        private readonly ICampaignRepository campaignRepository;
        private readonly ITemplateCampaignRepository templateCampaignRepository;

        public TemplateService(ITemplateRepository templateRepository,
            ICampaignRepository campaignRepository,
            ITemplateCampaignRepository templateCampaignRepository)
        {
            this.templateRepository = templateRepository;
            this.campaignRepository = campaignRepository;
            this.templateCampaignRepository = templateCampaignRepository;
        }


        public async Task<Response<IEnumerable<TemplateReadDto>>> GetAllTemplatesAsync(string CharityId)
        {
            var templates = await templateRepository.GetAllTemplatesByCharityIdAsync(CharityId);
            if (templates == null)
            {
                return new Response<IEnumerable<TemplateReadDto>>(HttpStatusCode.NotFound, "Template Details not found.");
            }

            return new Response<IEnumerable<TemplateReadDto>>(HttpStatusCode.OK, templates.Select(x => x.MapToDto()).ToList(), ResponseCode.Data);
        }

        public async Task<Response<TemplateReadDto>> GetTemplateByIdAsync(string id)
        {
            var template = await templateRepository.GetByIdAsync(id);
            if (template == null)
            {
                return new Response<TemplateReadDto>(HttpStatusCode.NotFound, "Template Details not found.");
            }

            return new Response<TemplateReadDto>(HttpStatusCode.OK, template.MapToDto(), ResponseCode.Data);
        }

        public async Task<Response> CreateTemplateAsync(TemplateDto request)
        {

            if (await templateRepository.ExistsWithCampaignAsync(request.CampaignId))
            {
                return new Response(HttpStatusCode.BadRequest, "Campaign already used in another template or mode.");
            }

            var template = new Template
            {
                Id = Guid.NewGuid().ToString(),
                CharityId = request.CharityId,
                Name = request.Name,
                Icon = request.Icon,
                Message = request.Message,
                CampaignId = request.CampaignId,
            };

            foreach (var mode in request.Modes)
            {
                if (await templateRepository.ExistsWithCampaignAsync(mode.CampaignId))
                {
                    return new Response(HttpStatusCode.BadRequest, "Campaign already used in another template or mode.");
                }
                else
                {
                    template.Modes.Add(new Mode
                    {
                        ModeType = mode.ModeType,
                        Amount = mode.Amount,
                        CampaignId = mode.CampaignId,
                        TemplateId = template.Id,
                    });
                }
            }

            if (await templateRepository.AddAsync(template))
            {
                return new Response(HttpStatusCode.Created, "Template Created");
            }

            return new Response(HttpStatusCode.BadRequest, "Template Creation Failed.");
        }

        public async Task<Response> UpdateTemplateAsync(TemplateUpdateDto request)
        {
            var template = await templateRepository.GetByIdAsync(request.TemplateId);
            if (template == null)
                return new Response(HttpStatusCode.NotFound, "Template Details not found.");

            if (await templateRepository.ExistsWithCampaignAsync(request.CampaignId) && request.CampaignId != template.CampaignId)
            {
                return new Response(HttpStatusCode.BadRequest, "Campaign already used in another template or mode.");
            }

            foreach (var mode in request.Modes)
            {
                if (await templateRepository.ExistsWithCampaignAsync(mode.CampaignId) &&
                    !template.Modes.Any(m => m.CampaignId == mode.CampaignId))
                    return new Response(HttpStatusCode.BadRequest, "Mode campaign already used");
            }

            template.Name = request.Name;
            template.CharityId = request.CharityId;
            template.CampaignId = request.CampaignId;

            template.Modes.Clear();
            foreach (var modeDto in request.Modes)
            {
                template.Modes.Add(new Mode
                {
                    ModeType = modeDto.ModeType,
                    CampaignId = modeDto.CampaignId,
                    Amount = modeDto.Amount,
                    TemplateId = template.Id
                });
            }

            if (await templateRepository.UpdateAsync(template))
            {
                return new Response(HttpStatusCode.OK, "Template Updated Successfully");
            }

            return new Response(HttpStatusCode.BadRequest, "Template Updating Failed.");
        }

        public async Task<Response> DeleteTemplateAsync(string id)
        {
            var template = await templateRepository.GetByIdAsync(id);
            if (template == null) return new Response(HttpStatusCode.BadRequest, "Template Details not found.");

            if (await templateRepository.DeleteAsync(template))
            {
                return new Response(HttpStatusCode.OK, "Template Deleted Successfully");
            }
            return new Response(HttpStatusCode.BadRequest, "Template Deleting Failed.");
        }
    }
}
