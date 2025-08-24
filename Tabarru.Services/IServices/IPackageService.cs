using Tabarru.Common.Models;
using Tabarru.Services.Models;

namespace Tabarru.Services.IServices
{
    public interface IPackageService
    {
        Task<Response<IList<PackageDetailsDto>>> GetAllPackagesAsync();
    }
}
