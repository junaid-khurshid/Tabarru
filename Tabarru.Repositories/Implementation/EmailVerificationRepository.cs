using Microsoft.EntityFrameworkCore;
using Tabarru.Repositories.DatabaseContext;
using Tabarru.Repositories.IRepository;
using Tabarru.Repositories.Models;

namespace Tabarru.Repositories.Implementation
{
    public class EmailVerificationRepository : IEmailVerificationRepository
    {
        private readonly DbStorageContext dbStorageContext;

        public EmailVerificationRepository(DbStorageContext dbStorageContext)
        {
            this.dbStorageContext = dbStorageContext;
        }

        public async Task<bool> AddAsync(EmailVerificationDetails emailVerificationDetails)
        {
            dbStorageContext.EmailVerificationDetails.Add(emailVerificationDetails);
            return await dbStorageContext.SaveChangesAsync() > 0;
        }

        public async Task<EmailVerificationDetails> GetByEmailAsync(string emailAddress)
        {
            return await dbStorageContext.EmailVerificationDetails.FirstOrDefaultAsync(x => x.Email.Equals(emailAddress));
        }

        public async Task<EmailVerificationDetails> GetByEmailAndIsNotUsedAsync(string emailAddress)
        {
            return await dbStorageContext.EmailVerificationDetails.FirstOrDefaultAsync(x => x.Email.Equals(emailAddress) && !x.IsUsed);
        }

        public async Task<EmailVerificationDetails> GetByIdAsync(string emailVerificationId)
        {
            return await dbStorageContext.EmailVerificationDetails.FindAsync(emailVerificationId);
        }

        public async Task<bool> UpdateAsync(EmailVerificationDetails emailVerificationDetails)
        {
            dbStorageContext.EmailVerificationDetails.Update(emailVerificationDetails);
            return await dbStorageContext.SaveChangesAsync() > 0;
        }
    }
}
