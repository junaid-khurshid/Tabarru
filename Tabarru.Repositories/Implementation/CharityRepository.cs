using Microsoft.EntityFrameworkCore;
using Tabarru.Common.Enums;
using Tabarru.Repositories.DatabaseContext;
using Tabarru.Repositories.IRepository;
using Tabarru.Repositories.Models;

namespace Tabarru.Repositories.Implementation
{
    public class CharityRepository : ICharityRepository
    {
        private readonly DbStorageContext dbStorageContext;

        public CharityRepository(DbStorageContext dbStorageContext)
        {
            this.dbStorageContext = dbStorageContext;
        }
        public async Task<bool> AddAsync(Charity charity)
        {
            dbStorageContext.Charities.Add(charity);
            return await dbStorageContext.SaveChangesAsync() > 0;
        }

        public async Task<Charity> GetByEmailAsync(string email)
        {
            return await dbStorageContext.Charities.FirstOrDefaultAsync(x => x.Email.Equals(email) && !x.IsDeleted);
        }

        public async Task<Charity> GetByIdAsync(string charityId)
        {
            return await dbStorageContext.Charities.FindAsync(charityId);
        }

        public async Task<bool> UpdateAsync(Charity charity)
        {
            dbStorageContext.Charities.Update(charity);
            return await dbStorageContext.SaveChangesAsync() > 0;
        }

        public async Task<CharityKycDetails> GetCharityKycDetailsAsync(string charityId)
        {
            return await dbStorageContext.CharityKycDetails
                .Include(x => x.CharityKycDocuments)
                .FirstOrDefaultAsync(x => x.CharityId == charityId && !x.IsDeleted);
        }

        public async Task<CharityKycDetails> GetCharityKycDetailsApprovedAsync(string charityId)
        {
            return await dbStorageContext.CharityKycDetails
                .Include(x => x.CharityKycDocuments)
                .FirstOrDefaultAsync(x => x.CharityId == charityId && x.Status.Equals(CharityKycStatus.Approved) && !x.IsDeleted);
        }

        public async Task<Charity> GetCharityAllDetailsByIdAsync(string charityId)
        {
            return await dbStorageContext.Charities
                .Include(c => c.CharityKycDetails)
                .ThenInclude(k => k.CharityKycDocuments)
                .FirstOrDefaultAsync(x => x.Id.Equals(charityId));
        }
    }
}