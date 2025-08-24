using System.Net;
using Tabarru.Common.Enums;
using Tabarru.Common.Models;
using Tabarru.Repositories.IRepository;
using Tabarru.Services.IServices;
using Tabarru.Services.Models;

namespace Tabarru.Services.Implementation
{
    public class PackageService : IPackageService
    {
        private readonly IPackageRepository packageRepository;

        public PackageService(IPackageRepository packageRepository)
        {
            this.packageRepository = packageRepository;
        }

        public async Task<Response<IList<PackageDetailsDto>>> GetAllPackagesAsync()
        {
            var result = (await packageRepository.GetAllAsync()).ToList().Select(p => p.MapToDto()).ToList();
            return new Response<IList<PackageDetailsDto>>(HttpStatusCode.OK, result, ResponseCode.Data);
        }
    }
}
