using Tabarru.Common.Enums;
using Tabarru.Common.Models;
using Tabarru.Repositories.Models;
using Tabarru.Services.Models;

namespace Tabarru.Services.IServices
{
    public interface ICharityKycService
    {
        Task<Response> SubmitKycAsync(string charityId, CharityKycDto dto);
        Task<Response> UpdateKycStatusAsync(AdminKycUpdateDto dto);
        Task<Response<IList<CharityReadDto>>> GetAllCharitiesForAdminAsync();
        Task<CharityKycStatus> GetCharityKycStatus(string CharityId);
    }
}
