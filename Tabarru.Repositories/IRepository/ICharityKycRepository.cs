using Tabarru.Common.Enums;
using Tabarru.Repositories.Models;

namespace Tabarru.Repositories.IRepository
{
    public interface ICharityKycRepository
    {
        Task<Charity> GetCharityByIdAsync(string charityId);
        Task<CharityKycDetails> GetCharityKycDetailsByCharityIdAsync(string charityId);
        Task<IEnumerable<Charity>> GetAllCharitiesAsync();
        Task<bool> AddAsync(CharityKycDetails details);
        Task<bool> UpdateAsync(CharityKycDetails charityKycDetails);
        Task<CharityKycStatus> GetCharityKycStatus(string CharityId);
        Task<CharityKycDetails> GetActiveByCharityIdAsync(string charityId);
        Task<bool> DeleteAsync(CharityKycDetails charityKycDetails);
        Task<bool> DeleteKycDocumentAsync(CharityKycDocuments charityKycDocuments);
    }
}
