using Tabarru.Repositories.Models;

namespace Tabarru.Repositories.IRepository
{
    public interface IPackageRepository
    {
        Task<bool> AddAsync(PackageDetails accountFeeDetail);

        Task<IEnumerable<PackageDetails>> GetAllAsync();
    }
}
