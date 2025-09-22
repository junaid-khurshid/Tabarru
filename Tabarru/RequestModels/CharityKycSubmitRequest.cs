using System.ComponentModel.DataAnnotations;
using Tabarru.Services.Models;

namespace Tabarru.RequestModels
{
    public class CharityKycSubmitRequest
    {
        [Required(ErrorMessage = "REQUIRED")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "REQUIRED")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "REQUIRED")]
        public string CharityName { get; set; }
        [Required(ErrorMessage = "REQUIRED")]
        public string CountryCode { get; set; }
        [Required(ErrorMessage = "REQUIRED")]
        public string CharityNumber { get; set; }

        // Documents
        [Required(ErrorMessage = "REQUIRED")]
        public IFormFile Logo { get; set; }
        [Required(ErrorMessage = "REQUIRED")]
        public IFormFile IncorporationCertificate { get; set; }
        public IFormFile? UtilityBill { get; set; }
        public IFormFile? TaxExemptionCertificate { get; set; }
        public IFormFile? BankStatement { get; set; }
    }


    static class CharityKycExtension
    {
        public static CharityKycDto MapToDto(this CharityKycSubmitRequest request)
        {
            return new CharityKycDto
            {
                BankStatement = request.BankStatement,
                FirstName = request.FirstName,
                CharityName = request.CharityName,
                CharityNumber = request.CharityNumber,
                CountryCode = request.CountryCode,
                IncorporationCertificate = request.IncorporationCertificate,
                LastName = request.LastName,
                Logo = request.Logo,
                TaxExemptionCertificate = request.TaxExemptionCertificate,
                UtilityBill = request.UtilityBill,
            };
        }
    }
}
