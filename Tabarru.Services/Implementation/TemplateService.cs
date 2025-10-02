using System.Net;
using Tabarru.Common.Enums;
using Tabarru.Common.Helper;
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
        private readonly ICharityRepository charityRepository;

        public TemplateService(ITemplateRepository templateRepository,
            ICampaignRepository campaignRepository,
            ICharityRepository charityRepository)
        {
            this.templateRepository = templateRepository;
            this.campaignRepository = campaignRepository;
            this.charityRepository = charityRepository;
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

            var dto = new TemplateReadDto
            {
                Id = template.Id,
                Name = template.Name,
                CharityId = template.CharityId,
                Icon = template.Icon,
                Message = template.Message,
                Modes = template.Modes.Where(m => !m.IsDeleted).Select(mode => new ModeReadDto
                {
                    Id = mode.Id,
                    ModeType = mode.ModeType,
                    CampaignId = mode.CampaignId,
                    Amount = mode.ModeType == Modes.Default ? mode.Campaign.ListOfAmounts : mode.Amount.ToString()
                }).ToList()
            };

            return new Response<TemplateReadDto>(HttpStatusCode.OK, dto, ResponseCode.Data);
        }

        public async Task<Response> CreateTemplateAsync(TemplateDto request)
        {
            var charity = await charityRepository.GetByIdAsync(request.CharityId);
            if (charity == null)
                return new Response(HttpStatusCode.NotFound, "Charity not found");

            if (request.Modes.Count() == 0)
            {
                return new Response(HttpStatusCode.BadRequest, "Modes should not be empty");
            }

            var template = new Template
            {
                Id = Guid.NewGuid().ToString(),
                CharityId = request.CharityId,
                Name = request.Name,
                Icon = await request.Icon.ConvertFileToBase64Async(),
                Message = request.Message,
            };

            foreach (var mode in request.Modes)
            {

                if (await campaignRepository.GetByIdAsync(mode.CampaignId) == null)
                {
                    return new Response(HttpStatusCode.BadRequest, $"Campaign with {mode.CampaignId} not found");
                }

                if (await templateRepository.ExistsWithCampaignAsync(mode.CampaignId))
                {
                    return new Response(HttpStatusCode.BadRequest, "Campaign already used in another template or mode.");
                }
                else
                {
                    template.Modes.Add(new Mode
                    {
                        ModeType = mode.ModeType,
                        Amount = mode.ModeType == Modes.Default ? 0 : mode.Amount,
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
            var charity = await charityRepository.GetByIdAsync(request.CharityId);
            if (charity == null)
                return new Response(HttpStatusCode.NotFound, "Charity not found");

            var template = await templateRepository.GetByIdAsync(request.TemplateId);
            if (template == null)
                return new Response(HttpStatusCode.NotFound, "Template Details not found.");


            template.Name = request.Name;
            template.CharityId = request.CharityId;
            template.Icon = await request.Icon.ConvertFileToBase64Async();
            template.Message = request.Message;

            var newModes = new List<Mode>();
            foreach (var modeDto in request.Modes)
            {
                if (await templateRepository.ExistsWithCampaignAsync(modeDto.CampaignId) &&
                    !template.Modes.Any(m => m.CampaignId == modeDto.CampaignId))
                    return new Response(HttpStatusCode.BadRequest, $"Mode campaign {modeDto.CampaignId} already used");

                newModes.Add(new Mode
                {
                    ModeType = modeDto.ModeType,
                    CampaignId = modeDto.CampaignId,
                    Amount = modeDto.ModeType == Modes.Default ? 0 : modeDto.Amount,
                    TemplateId = template.Id,
                });
            }

            if (await templateRepository.DeleteModesAsync(template.Modes.ToList()))
            {
                return new Response(HttpStatusCode.OK, "Template Updating Failed");
            }

            template.Modes.Clear();
            template.Modes = newModes;

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
