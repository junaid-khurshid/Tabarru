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
        private readonly ITemplateRepository templateRepository;

        public CampaignService(ICampaignRepository campaignRepository,
             DbStorageContext dbContext,
             ITemplateRepository templateRepository)

        {
            this.campaignRepository = campaignRepository;
            this.dbContext = dbContext;
            this.templateRepository = templateRepository;
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
                if (currentDefault != null)
                {
                    currentDefault.IsDefault = false;
                    await campaignRepository.UpdateAsync(currentDefault);
                }
            }

            var campaign = new Campaign
            {
                CharityId = dto.CharityId,
                Name = dto.Name.Trim(),
                Icon = await dto.Icon.ConvertFileToBase64Async(),
                ListOfAmounts = dto.ListOfAmounts,
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

        public async Task<Response> UpdateAsync(CampaignDto dto)
        {
            var campaign = await campaignRepository.GetByIdAsync(dto.Id);
            if (campaign == null)
                return new Response(HttpStatusCode.NotFound, "Campaign not found.");

            var existing = await campaignRepository.GetByNameAndCharityIdAsync(dto.Name, dto.CharityId);
            if (existing != null && existing.Id != dto.Id)
                return new Response(HttpStatusCode.BadRequest, "A campaign with the same name already exists.");

            if (dto.IsDefault)
            {
                var currentDefault = await campaignRepository.GetAllByCharityIdAndDefaultOneOnlyAsync(dto.CharityId);
                if (currentDefault != null && currentDefault.Id != dto.Id)
                {
                    currentDefault.IsDefault = false;
                    await campaignRepository.UpdateAsync(currentDefault);
                }
            }

            campaign.Name = dto.Name.Trim();
            campaign.ListOfAmounts = dto.ListOfAmounts;
            campaign.IsEnabled = dto.IsEnabled;
            campaign.IsDefault = dto.IsDefault;

            if (dto.Icon != null)
            {
                campaign.Icon = await dto.Icon.ConvertFileToBase64Async();
            }

            var updated = await campaignRepository.UpdateAsync(campaign);

            if (!updated)
            {
                return new Response(HttpStatusCode.BadRequest, "Campaign Update Failed.");
            }

            return new Response(HttpStatusCode.OK, "Campaign Updated Successfully.");
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
                return new Response<CampaignReadDto>(HttpStatusCode.BadRequest, $"Campaign with {id} not found");
            }

            return new Response<CampaignReadDto>(HttpStatusCode.OK, campaign.MapToDto(), ResponseCode.Data);
        }

        public async Task<Response> UpdateStatusAsync(CampaignUpdateStatusDto dto)
        {
            var campaign = await campaignRepository.GetByCampaignIdAndCharityIdOnlyAsync(dto.CampaignId, dto.CharityId);
            if (campaign == null)
            {
                return new Response(HttpStatusCode.BadRequest, "Campaign Details not found");
            }

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

                if (await campaignRepository.UpdateAsync(campaign))
                {
                    await transaction.CommitAsync();
                    return new Response(HttpStatusCode.OK, "Campaign Updated Successfully");
                }

                await transaction.RollbackAsync();
                return new Response(HttpStatusCode.OK, "Campaign Updating Failed.");
            }
        }

        public async Task<Response> DeleteCampaignAsync(string id)
        {
            var campaign = await campaignRepository.GetByIdAsync(id);
            if (campaign == null) return new Response(HttpStatusCode.BadRequest, "Campaign Details not found.");

            if (await templateRepository.ExistsWithCampaignAsync(id))
            {
                return new Response(HttpStatusCode.BadRequest, $"Cannot delete campaign {id}, as its already used with Modes");
            }

            if (await campaignRepository.DeleteAsync(campaign))
            {
                return new Response(HttpStatusCode.OK, "Campaign Deleted Successfully");
            }
            return new Response(HttpStatusCode.BadRequest, "Campaign Deleting Failed.");
        }
    }
}