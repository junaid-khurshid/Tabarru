using Microsoft.AspNetCore.Http;
using Tabarru.Repositories.Models;

namespace Tabarru.Services.Models
{
    public class CampaignDto
    {
        public string Id { get; set; }
        public string CharityId { get; set; }
        public string Name { get; set; }
        public IFormFile Icon { get; set; }
        public string ListOfAmounts { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsDefault { get; set; }
    }

    static class CampaignExtension
    {
        public static CampaignReadDto MapToDto(this Campaign campaign)
        {
            return new CampaignReadDto
            {
                Id = campaign.Id,
                CharityId = campaign.CharityId,
                Name = campaign.Name,
                Icon = campaign.Icon,
                ListOfAmounts = campaign.ListOfAmounts,
                IsEnabled = campaign.IsEnabled,
                IsDefault = campaign.IsDefault,
            };
        }
    }

    public class CampaignReadDto
    {
        public string Id { get; set; }
        public string CharityId { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string ListOfAmounts { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsDefault { get; set; }
    }
}
