using System.Net;
using Tabarru.Common.Enums;
using Tabarru.Common.Models;
using Tabarru.Repositories.IRepository;
using Tabarru.Repositories.Models;
using Tabarru.Services.IServices;
using Tabarru.Services.Models;

namespace Tabarru.Services.Implementation
{
    public class CharityKycService : ICharityKycService
    {
        private readonly ICharityKycRepository charityKycRepository;
        private readonly ICharityRepository charityRepository;

        public CharityKycService(ICharityKycRepository charityKycRepository,
            ICharityRepository charityRepository)
        {
            this.charityKycRepository = charityKycRepository;
            this.charityRepository = charityRepository;
        }

        public async Task<Response> SubmitKycAsync(string charityId, CharityKycDto dto)
        {
            var charity = await charityKycRepository.GetCharityByIdAsync(charityId);
            if (charity == null)
                return new Response(HttpStatusCode.NotFound, "Charity not found");

            var kycDetails = new CharityKycDetails
            {
                CharityId = charityId,
                Status = CharityKycStatus.Pending,
                IsCharityDocumentUploaded = !string.IsNullOrEmpty(dto.IncorporationCertificate),
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                CharityName = dto.CharityName,
                CountryCode = dto.CountryCode,
                CharityNumber = dto.CharityNumber,
                CharityKycDocuments = new CharityKycDocuments
                {
                    Logo = dto.Logo,
                    IncorporationCertificate = dto.IncorporationCertificate,
                    UtilityBill = dto.UtilityBill,
                    TaxExemptionCertificate = dto.TaxExemptionCertificate,
                    BankStatement = dto.BankStatement
                }
            };

            charity.KycStatus = CharityKycStatus.Pending;
            charity.IsKycVerified = false;

            var added = await charityKycRepository.AddAsync(kycDetails);

            if (!added)
            {
                return new Response(HttpStatusCode.BadRequest, "Charity KYC submission failed.");
            }

            return new Response(HttpStatusCode.OK, "Charity KYC submitted successfully");
        }

        public async Task<Response> UpdateKycStatusAsync(AdminKycUpdateDto dto)
        {
            var charity = await charityKycRepository.GetCharityByIdAsync(dto.CharityId);
            if (charity == null)
                return new Response(HttpStatusCode.NotFound, "Charity not found");

            var kycDetails = await charityKycRepository.GetCharityKycDetailsByCharityIdAsync(dto.CharityId);
            if (kycDetails == null)
                return new Response(HttpStatusCode.NotFound, "KYC Details not found");

            charity.KycStatus = dto.Status;
            kycDetails.Status = dto.Status;
            charity.IsKycVerified = dto.Status == CharityKycStatus.Approved;

            var charityUdpate = await charityRepository.UpdateAsync(charity);
            var charityKycUpdate = await charityKycRepository.UpdateAsync(kycDetails);

            if (!charityUdpate || !charityKycUpdate)
            {
                return new Response(HttpStatusCode.BadRequest, "Charity KYC submission failed.");
            }

            return new Response(HttpStatusCode.OK, $"Charity KYC {dto.Status} Updated");
        }

        public async Task<Response<IList<Charity>>> GetAllCharitiesForAdminAsync()
        {
            var charities = await charityKycRepository.GetAllCharitiesAsync();

            return new Response<IList<Charity>>(HttpStatusCode.OK, charities.ToList(), ResponseCode.Data);
        }

        public Task<CharityKycStatus> GetCharityKycStatus(string CharityId)
        {
            return charityKycRepository.GetCharityKycStatus(CharityId);
        }
    }
}
