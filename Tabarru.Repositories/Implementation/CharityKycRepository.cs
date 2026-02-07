using Microsoft.EntityFrameworkCore;
using Tabarru.Common.Enums;
using Tabarru.Repositories.DatabaseContext;
using Tabarru.Repositories.IRepository;
using Tabarru.Repositories.Models;

namespace Tabarru.Repositories.Implementation
{
    public class CharityKycRepository : ICharityKycRepository
    {
        private readonly DbStorageContext dbStorageContext;

        public CharityKycRepository(DbStorageContext dbStorageContext)
        {
            this.dbStorageContext = dbStorageContext;
        }

        public async Task<Charity> GetCharityByIdAsync(string charityId)
        {
            return await dbStorageContext.Charities
                .FirstOrDefaultAsync(c => c.Id == charityId);
        }

        public async Task<CharityKycDetails?> GetActiveByCharityIdAsync(string charityId)
        {
            return await dbStorageContext.CharityKycDetails
                .Include(k => k.CharityKycDocuments)
                .FirstOrDefaultAsync(x =>
                    x.CharityId == charityId &&
                    !x.IsDeleted);
        }

        public async Task<CharityKycDetails> GetCharityKycDetailsByCharityIdAsync(string charityId)
        {
            return await dbStorageContext.CharityKycDetails
                .Include(k => k.CharityKycDocuments)
                .FirstOrDefaultAsync(k => k.CharityId == charityId);
        }

        public async Task<IEnumerable<Charity>> GetAllCharitiesAsync()
        {
            return await dbStorageContext.Charities
                .Include(c => c.CharityKycDetails)
                .ThenInclude(k => k.CharityKycDocuments)
                .ToListAsync();
        }

        public async Task<bool> AddAsync(CharityKycDetails details)
        {
            await dbStorageContext.CharityKycDetails.AddAsync(details);
            return await dbStorageContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(CharityKycDetails charityKycDetails)
        {
            dbStorageContext.CharityKycDetails.Update(charityKycDetails);
            return await dbStorageContext.SaveChangesAsync() > 0;
        }

        public async Task<CharityKycStatus> GetCharityKycStatus(string CharityId)
        {
            return await dbStorageContext.Charities.Where(x => x.Id.Equals(CharityId)).Select(x => x.KycStatus).FirstOrDefaultAsync();
        }

        public async Task<bool> DeleteAsync(CharityKycDetails charityKycDetails)
        {
            dbStorageContext.CharityKycDetails.Remove(charityKycDetails);
            return await dbStorageContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteKycDocumentAsync(CharityKycDocuments charityKycDocuments)
        {
            dbStorageContext.CharityKycDocuments.Remove(charityKycDocuments);
            return await dbStorageContext.SaveChangesAsync() > 0;
        }
    }
}
