using Microsoft.AspNetCore.Http;

namespace Tabarru.Services.Models
{
    public class CharityKycDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CharityName { get; set; }
        public string CountryCode { get; set; }
        public string CharityNumber { get; set; }

        // Documents
        public IFormFile Logo { get; set; }
        public IFormFile IncorporationCertificate { get; set; }
        public IFormFile? UtilityBill { get; set; }
        public IFormFile? TaxExemptionCertificate { get; set; }
        public IFormFile? BankStatement { get; set; }
    }
}
