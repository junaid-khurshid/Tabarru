using Microsoft.EntityFrameworkCore;
using Tabarru.Repositories.DatabaseContext;
using Tabarru.Repositories.IRepository;
using Tabarru.Repositories.Models;

namespace Tabarru.Repositories.Implementation
{
    public class PackageRepository : IPackageRepository
    {
        private readonly DbStorageContext dbStorageContext;

        public PackageRepository(DbStorageContext dbStorageContext)
        {
            this.dbStorageContext = dbStorageContext;
        }

        public async Task<bool> AddAsync(PackageDetails packageDetails)
        {
            dbStorageContext.PackageDetails.Add(packageDetails);
            return await dbStorageContext.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<PackageDetails>> GetAllAsync()
        {
            return await dbStorageContext.PackageDetails.ToListAsync();
        }
    }
}
