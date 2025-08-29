using Tabarru.Common.Enums;

namespace Tabarru.Services.Models
{
    public class CharityDetailDto
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public CharityKycStatus KycStatus { get; set; }

        public bool EmailVerified { get; set; }

        public string Role { get; set; }
    }
}
