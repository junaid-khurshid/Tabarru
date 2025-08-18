using System.Net;
using Tabarru.Common.Enums;
using Tabarru.Common.Models;
using Tabarru.Repositories.IRepository;
using Tabarru.Repositories.Models;
using Tabarru.Services.IServices;

namespace Tabarru.Services.Implementation
{
    public class PackageService : IPackageService
    {
        private readonly IPackageRepository packageRepository;

        public PackageService(IPackageRepository packageRepository)
        {
            this.packageRepository = packageRepository;
        }

        public async Task<Response<IList<PackageDetails>>> GetAllPackagesAsync()
        {
            return new Response<IList<PackageDetails>>(HttpStatusCode.OK, (await packageRepository.GetAllAsync()).ToList(), ResponseCode.Data);
        }
    }
}
