using System.Net;
using Tabarru.Common.Enums;
using Tabarru.Common.Helper;
using Tabarru.Common.Models;
using Tabarru.Repositories.DatabaseContext;
using Tabarru.Repositories.IRepository;
using Tabarru.Repositories.Models;
using Tabarru.Services.IServices;
using Tabarru.Services.Models;

namespace Tabarru.Services.Implementation
{
    public class CampaignService : ICampaignService
    {
        private readonly ICampaignRepository campaignRepository;
        private readonly DbStorageContext dbContext;

        public CampaignService(ICampaignRepository campaignRepository,
             DbStorageContext dbContext)
        {
            this.campaignRepository = campaignRepository;
            this.dbContext = dbContext;
        }

        public async Task<Response> CreateAsync(CampaignDto dto)
        {
            // 1. unique name check
            var existing = await campaignRepository.GetByNameAndCharityIdAsync(dto.Name, dto.CharityId);
            if (existing != null)
                return new Response(HttpStatusCode.BadRequest, "A campaign with the same name already exists.");

            // 2. if dto.IsDefault true, unset any existing default campaigns
            if (dto.IsDefault)
            {
                var currentDefault = await campaignRepository.GetAllByCharityIdAndDefaultOneOnlyAsync(dto.CharityId);

                currentDefault.IsDefault = false;
                await campaignRepository.UpdateAsync(currentDefault);
            }

            var campaign = new Campaign
            {
                CharityId = dto.CharityId,
                Name = dto.Name.Trim(),
                Icon = await dto.Icon.ConvertFileToBase64Async(),
                ListOfAmounts = dto.ListOfAmounts
                IsEnabled = dto.IsEnabled,
                IsDefault = dto.IsDefault,
            };

            var added = await campaignRepository.AddAsync(campaign);

            if (!added)
            {
                return new Response(HttpStatusCode.BadRequest, "Campaign Creation Failed.");
            }

            return new Response(HttpStatusCode.Created, "Campaign Creation Successfully.");
        }

        public async Task<Response<IList<CampaignReadDto>>> GetAllByCharityIDAsync(string CharityId)
        {
            var campaigns = await campaignRepository.GetAllByCharityIdAsync(CharityId);
            var campaignsList = campaigns.Select(i => i.MapToDto()).ToList();

            return new Response<IList<CampaignReadDto>>(HttpStatusCode.OK, campaignsList, ResponseCode.Data);
        }

        public async Task<Response<CampaignReadDto>> GetByIdAsync(string id)
        {
            var campaign = await campaignRepository.GetByIdAsync(id);
            if (campaign == null)
            {
                return new Response<CampaignReadDto>(HttpStatusCode.BadRequest, "Campaign Details not found");
            }

            return new Response<CampaignReadDto>(HttpStatusCode.OK, campaign.MapToDto(), ResponseCode.Data);
        }

        public async Task<Response<CampaignReadDto>> UpdateStatusAsync(CampaignUpdateStatusDto dto)
        {
            var campaign = await campaignRepository.GetAllByCampaignIdAndCharityIdOnlyAsync(dto.CampaignId, dto.CharityId);
            if (campaign == null)
            {
                return new Response<CampaignReadDto>(HttpStatusCode.BadRequest, "Campaign Details not found");
            }

            // If setting IsDefault to true, unset default on others

            using (var transaction = await dbContext.Database.BeginTransactionAsync())
            {

                if (dto.IsDefault)
                {
                    var campaignDefaulted = await campaignRepository.GetAllByCharityIdAndDefaultOneOnlyAsync(dto.CharityId);
                    if (campaignDefaulted.Id != dto.CampaignId)
                    {

                        campaignDefaulted.IsDefault = false;
                        await campaignRepository.UpdateAsync(campaignDefaulted);
                    }
                }

                campaign.IsEnabled = dto.IsEnabled;
                campaign.IsDefault = dto.IsDefault;

                await campaignRepository.UpdateAsync(campaign);

                return new Response<CampaignReadDto>(HttpStatusCode.OK, campaign.MapToDto(), ResponseCode.Data);
            }
        }
    }
}