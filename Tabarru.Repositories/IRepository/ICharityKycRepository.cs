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
    }
}
