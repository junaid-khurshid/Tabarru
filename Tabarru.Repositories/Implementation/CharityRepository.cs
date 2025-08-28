using Microsoft.EntityFrameworkCore;
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
            dbStorageContext.Charity.Add(charity);
            return await dbStorageContext.SaveChangesAsync() > 0;
        }

        public async Task<Charity> GetByEmailAsync(string email)
        {
            return await dbStorageContext.Charity.FirstOrDefaultAsync(x => x.Email.Equals(email));
        }

        public async Task<Charity> GetByIdAsync(string charityId)
        {
            return await dbStorageContext.Charity.FindAsync(charityId);
        }

        public async Task<bool> UpdateAsync(Charity charity)
        {
            dbStorageContext.Charity.Update(charity);
            return await dbStorageContext.SaveChangesAsync() > 0;
        }
    }
}