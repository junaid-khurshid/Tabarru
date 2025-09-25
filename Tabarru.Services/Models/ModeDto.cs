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
        public static ModeReadDto MapToDto(this Mode mode)
        {
            return new ModeReadDto
            {
                Id = mode.Id,
                ModeType = mode.ModeType,
                Amount = mode.ModeType == Modes.Default ? mode.Campaign.ListOfAmounts : mode.Amount.ToString(),
                CampaignId = mode.CampaignId,
            };
        }
    }

    public class ModeReadDto
    {
        public string Id { get; set; }
        public Modes ModeType { get; set; } // enum string
        public string Amount { get; set; }
        public string CampaignId { get; set; }
    }
}