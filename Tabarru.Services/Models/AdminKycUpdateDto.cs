using Tabarru.Common.Enums;

namespace Tabarru.Services.Models
{
    public class AdminKycUpdateDto
    {
        public string CharityId { get; set; }
        public CharityKycStatus Status { get; set; }
        public string Reason { get; set; }
    }
}
