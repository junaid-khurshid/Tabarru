namespace Tabarru.Services.Models
{
    public class CampaignUpdateStatusDto
    {
        public string CampaignId { get; set; }

        public string CharityId { get; set; }

        public bool IsEnabled { get; set; }

        public bool IsDefault { get; set; }
    }
}
