using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tabarru.Common.Helper;
using Tabarru.Common.Models;
using Tabarru.RequestModels;
using Tabarru.Services.IServices;
using Tabarru.Services.Models;

namespace Tabarru.Controllers
{
    [Authorize(Policy = "KycApprovedOnly", Roles = "USER,ADMIN")]
    [Route("api/[controller]")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private readonly IDeviceService deviceService;

        public DevicesController(IDeviceService deviceService)
        {
            this.deviceService = deviceService;
        }

        [HttpPost]
        public async Task<Response> AddDevice([FromBody] DeviceCreateRequest deviceCreateRequest)
        {
            return await deviceService.AddDeviceAsync(deviceCreateRequest.MaptoDto(TokenClaimHelper.GetId(User)));
        }

        [HttpPut("{id}")]
        public async Task<Response<DeviceReadDto>> UpdateDevice(string id, [FromBody] DeviceCreateRequest deviceUpdateRequest)
        {
            return await deviceService.UpdateDeviceAsync(id, deviceUpdateRequest.MaptoDto(TokenClaimHelper.GetId(User)));
        }

        [HttpDelete("{id}")]
        public async Task<Response> DeleteDevice(string id)
        {
            return await deviceService.DeleteDeviceAsync(id);
        }

        [HttpGet]
        public async Task<Response<IList<DeviceReadDto>>> GetAllDevices()
        {
            return await deviceService.GetAllDevicesAsync(TokenClaimHelper.GetId(User));
        }

        [HttpGet("{id}")]
        public async Task<Response<DeviceReadDto>> GetDeviceById(string id)
        {
            return await deviceService.GetDeviceByIdAsync(id);
        }
    }
}
