using System.Data;
using Tabarru.Common.Enums;
using Tabarru.Repositories.Models;

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

    public class CharityReadDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string KycStatus { get; set; }
        public bool EmailVerified { get; set; }
        public bool IsKycVerified { get; set; }
        public bool IsPackageVerified { get; set; }
        public string PackageId { get; set; }

        public List<CharityKycDetailsReadDto> CharityKycDetails { get; set; }
    }


    static class CharityDtoExtension
    {
        public static CharityReadDto MapToDto(this Charity charity)
        {
            return new CharityReadDto
            {
                Id = charity.Id,
                Email = charity.Email,
                Role = charity.Role,
                KycStatus = charity.KycStatus.ToString(),
                EmailVerified = charity.EmailVerified,
                IsKycVerified = charity.IsKycVerified,
                IsPackageVerified = charity.IsPackageVerified,
                PackageId = charity.PackageId,
                CharityKycDetails = charity.CharityKycDetails.Select(k => new CharityKycDetailsReadDto
                {
                    Id = k.Id,
                    CharityId =  charity.Id,
                    Status = k.Status.ToString(),
                    IsCharityDocumentUploaded = k.IsCharityDocumentUploaded,
                    FirstName = k.FirstName,
                    LastName = k.LastName,
                    CharityName = k.CharityName,
                    CountryCode = k.CountryCode,
                    CharityNumber = k.CharityNumber,
                    CharityKycDocuments = k.CharityKycDocuments is null ? null : new CharityKycDocumentsReadDto
                    {
                        Id = k.CharityKycDocuments.Id,
                        Logo = k.CharityKycDocuments.Logo,
                        IncorporationCertificate = k.CharityKycDocuments.IncorporationCertificate,
                        UtilityBill = k.CharityKycDocuments.UtilityBill,
                        TaxExemptionCertificate = k.CharityKycDocuments.TaxExemptionCertificate,
                        BankStatement = k.CharityKycDocuments.BankStatement
                    }
                }).ToList()
            };
        }

    }
}