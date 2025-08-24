using Microsoft.AspNetCore.Mvc;
using Tabarru.Common.Models;
using Tabarru.Services.IServices;
using Tabarru.Services.Models;

namespace Tabarru.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackagesController : ControllerBase
    {
        private readonly IPackageService packageService;

        public PackagesController(IPackageService packageService)
        {
            this.packageService = packageService;
        }

        [HttpGet]
        public async Task<Response<IList<PackageDetailsDto>>> GetPackages()
        {
            return await this.packageService.GetAllPackagesAsync();
        }
    }
}
