using Tabarru.Common.Enums;
using Tabarru.Repositories.Models;

namespace Tabarru.Services.Models
{
    public class ModeDto
    {
        public Modes ModeType { get; set; }
        public int Amount { get; set; }
        public string CampaignId { get; set; }
    }


    static class ModeExtension
    {
        public static ModeReadDto MapToDto(this Mode campaign)
        {
            return new ModeReadDto
            {
                Id = campaign.Id,
                ModeType = campaign.ModeType,
                Amount = campaign.Amount,
                CampaignId = campaign.CampaignId,
            };
        }
    }

    public class ModeReadDto
    {
        public string Id { get; set; }
        public Modes ModeType { get; set; } // enum string
        public int Amount { get; set; }
        public string CampaignId { get; set; }
    }
}