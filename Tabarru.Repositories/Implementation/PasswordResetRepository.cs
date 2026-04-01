using Microsoft.EntityFrameworkCore;
using Tabarru.Repositories.DatabaseContext;
using Tabarru.Repositories.IRepository;
using Tabarru.Repositories.Models;

namespace Tabarru.Repositories.Implementation
{
    public class PasswordResetRepository : IPasswordResetRepository
    {
        private readonly DbStorageContext context;

        public PasswordResetRepository(DbStorageContext context)
        {
            this.context = context;
        }

        public async Task<bool> AddAsync(PasswordResetToken token)
        {
            await context.PasswordResetTokens.AddAsync(token);
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<PasswordResetToken> GetValidTokenAsync(string token)
        {
            return await context.PasswordResetTokens
                .Include(x => x.Charity)
                .FirstOrDefaultAsync(x =>
                    x.Token == token &&
                    !x.IsUsed &&
                    x.ExpiryTime > DateTime.UtcNow &&
                    !x.IsDeleted);
        }

        public async Task InvalidateExistingTokensAsync(string charityId)
        {
            var tokens = await context.PasswordResetTokens
                .Where(x => x.CharityId == charityId && !x.IsUsed && x.ExpiryTime > DateTime.UtcNow)
                .ToListAsync();

            foreach (var token in tokens)
            {
                token.IsUsed = true;
            }

            await context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(PasswordResetToken token)
        {
            context.PasswordResetTokens.Update(token);
            return await context.SaveChangesAsync() > 0;

        }
    }
}
