using Tabarru.Repositories.Models;

namespace Tabarru.Repositories.IRepository
{
    public interface ICharityRepository
    {
        Task<Charity> GetByIdAsync(string charityId);
        Task<Charity> GetByEmailAsync(string charityId);
        Task<bool> AddAsync(Charity charity);
        Task<bool> UpdateAsync(Charity charity);
    }
}
