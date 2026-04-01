using Tabarru.Repositories.Models;

namespace Tabarru.Repositories.IRepository
{
    public interface IPasswordResetRepository
    {
        Task<bool> AddAsync(PasswordResetToken token);
        Task<PasswordResetToken> GetValidTokenAsync(string token);
        Task<bool> UpdateAsync(PasswordResetToken token);
        Task InvalidateExistingTokensAsync(string charityId);
    }
}
