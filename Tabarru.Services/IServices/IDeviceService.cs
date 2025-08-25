using Tabarru.Common.Models;
using Tabarru.Services.Models;

namespace Tabarru.Services.IServices
{
    public interface IDeviceService
    {
        Task<Response> AddDeviceAsync(DeviceDto dto);
        Task<Response<IList<DeviceReadDto>>> GetAllDevicesAsync(string CharityId);
        Task<Response<DeviceReadDto>> GetDeviceByIdAsync(string Id);
        Task<Response<DeviceReadDto>> UpdateDeviceAsync(string id, DeviceDto dto);
        Task<Response> DeleteDeviceAsync(string id);
    }
}
