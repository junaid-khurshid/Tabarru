using Tabarru.Common.Enums;
using Tabarru.Services.Models;

namespace Tabarru.RequestModels
{
    public class ModeCreateRequest
    {
        public Modes ModeType { get; set; }
        public string CampaignId { get; set; }
        public int Amount { get; set; }
    }

    static class ModeExtension
    {
        public static ModeDto MapToDto(this ModeCreateRequest request)
        {
            return new ModeDto
            {
                Amount = request.Amount,
                CampaignId = request.CampaignId,
                ModeType = request.ModeType
            };
        }
    }
}
