using Tabarru.Services.Models;

namespace Tabarru.RequestModels
{
    public class UpdateCharityDetailsRequest
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string CharityName { get; set; }

        public string CharityNumber { get; set; }

        public string CountryCode { get; set; }

        public string Logo { get; set; }
    }

    static class CharityKycUpdateExtension
    {
        public static UpdateCharityDetailsDto MapToDto(this UpdateCharityDetailsRequest request, string CharityId)
        {
            return new UpdateCharityDetailsDto
            {
                CharityId = CharityId,
                FirstName = request.FirstName,
                CharityName = request.CharityName,
                CharityNumber = request.CharityNumber,
                CountryCode = request.CountryCode,
                LastName = request.LastName,
                Logo = request.Logo,
            };
        }
    }
}
