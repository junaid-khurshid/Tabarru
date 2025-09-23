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

    public class CharityKycDetailsReadDto
    {
        public string Id { get; set; }
        public string CharityId { get; set; }
        public string Status { get; set; }
        public bool IsCharityDocumentUploaded { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CharityName { get; set; }
        public string CountryCode { get; set; }
        public string CharityNumber { get; set; }

        public CharityKycDocumentsReadDto CharityKycDocuments { get; set; }
    }
    public class CharityKycDocumentsReadDto
    {
        public string Id { get; set; }
        public string CharityKycDetailsId { get; set; }
        public string Logo { get; set; }
        public string IncorporationCertificate { get; set; }
        public string UtilityBill { get; set; }
        public string TaxExemptionCertificate { get; set; }
        public string BankStatement { get; set; }
    }
}
