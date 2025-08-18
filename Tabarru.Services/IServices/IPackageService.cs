using Tabarru.Common.Models;
using Tabarru.Repositories.Models;

namespace Tabarru.Services.IServices
{
    public interface IPackageService
    {
        Task<Response<IList<PackageDetails>>> GetAllPackagesAsync();
    }
}
