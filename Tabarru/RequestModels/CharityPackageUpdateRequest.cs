using Tabarru.Services.Models;

namespace Tabarru.RequestModels
{
    public class CharityPackageUpdateRequest
    {
        public int PackageId { get; set; }
    }

    static class CharityPackageUpdateExtension
    {
        public static CharityPackageUpdateDto MaptoDto(this CharityPackageUpdateRequest charityPackageUpdateRequest, string CharityId)
        {
            return new CharityPackageUpdateDto
            {
                CharityId = CharityId,
                PackageId = charityPackageUpdateRequest.PackageId
            };
        }
    }
}
