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
        public string Logo { get; set; }
        public string IncorporationCertificate { get; set; }
        public string? UtilityBill { get; set; }
        public string? TaxExemptionCertificate { get; set; }
        public string? BankStatement { get; set; }
    }
}
