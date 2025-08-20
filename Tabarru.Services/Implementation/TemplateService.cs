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
                return new Response<IEnumerable<TemplateReadDto>>(HttpStatusCode.OK, new List<TemplateReadDto>(), ResponseCode.Data);
            }

            var res = templates.Select(t => new TemplateReadDto
            {
                Id = t.Id,
                Name = t.Name,
                Campaigns = t.TemplateCampaigns?.Select(tc => tc.Campaign.Name).ToList()
            });

            return new Response<IEnumerable<TemplateReadDto>>(HttpStatusCode.OK, res, ResponseCode.Data);
        }

        public async Task<Response<TemplateReadDto>> GetTemplateByIdAsync(string id)
        {
            var template = await templateRepository.GetByIdAsync(id);
            if (template == null)
            {
                return new Response<TemplateReadDto>(HttpStatusCode.OK, new TemplateReadDto(), ResponseCode.Data);
            }

            var res = new TemplateReadDto
            {
                Id = template.Id,
                Name = template.Name,
                Campaigns = template.TemplateCampaigns?.Select(tc => tc.Campaign.Name).ToList()
            };

            return new Response<TemplateReadDto>(HttpStatusCode.OK, res, ResponseCode.Data);
        }

        public async Task<Response> CreateTemplateAsync(TemplateDto request)
        {
            var template = new Template
            {
                Id = Guid.NewGuid().ToString(),
                CharityId = request.CharityId,
                Name = request.Name,
            };

            // Associate campaigns
            foreach (var campaignId in request.CampaignIds)
            {
                if (await campaignRepository.AnyByIdAsync(campaignId))
                {
                    template.TemplateCampaigns.Add(new TemplateCampaign
                    {
                        TemplateId = template.Id,
                        CampaignId = campaignId
                    });
                }
            }

            await templateRepository.AddAsync(template);

            return new Response(HttpStatusCode.Created, "Template Created");
        }

        public async Task<Response> UpdateTemplateAsync(TemplateUpdateDto request)
        {
            var template = await templateRepository.GetByIdAsync(request.TemplateId);
            if (template == null) return new Response(HttpStatusCode.BadRequest, "Template Details not found.");

            template.Name = request.Name;

            // Clear old associations
            if (await templateCampaignRepository.RemoveTemplateCampaignsRanges(template.TemplateCampaigns))
            {
                return new Response(HttpStatusCode.BadRequest, "Template Updating Failed.");
            }

            // Add new associations
            foreach (var campaignId in request.CampaignIds)
            {
                if (await campaignRepository.AnyByIdAsync(campaignId))
                {
                    template.TemplateCampaigns.Add(new TemplateCampaign
                    {
                        TemplateId = template.Id,
                        CampaignId = campaignId
                    });
                }
            }

            if (await templateRepository.UpdateAsync(template))
            {
                return new Response(HttpStatusCode.OK, "Updated Template Successfully");
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
