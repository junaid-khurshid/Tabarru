using System.Net;
using Tabarru.Common.Enums;
using Tabarru.Common.Models;
using Tabarru.Repositories.IRepository;
using Tabarru.Repositories.Models;
using Tabarru.Services.IServices;
using Tabarru.Services.Models;

namespace Tabarru.Services.Implementation
{
    public class DeviceService : IDeviceService
    {
        private readonly IDeviceRepository deviceRepository;

        public DeviceService(IDeviceRepository deviceRepository)
        {
            this.deviceRepository = deviceRepository;
        }

        public async Task<Response> AddDeviceAsync(DeviceDto dto)
        {
            var device = new Device
            {
                DeviceName = dto.DeviceName,
                CharityId = dto.CharityId,
                TemplateId = dto.TemplateId,
                DeviceLocation = dto.DeviceLocation,
            };

            var created = await deviceRepository.AddAsync(device);

            if (!created)
            {
                return new Response(HttpStatusCode.BadRequest, "Device Creation Failed.");
            }

            return new Response(HttpStatusCode.Created, "Device Created Successfully");
        }

        public async Task<Response<DeviceReadDto>> UpdateDeviceAsync(string id, DeviceDto dto)
        {
            var device = await deviceRepository.GetByIdAsync(id);
            if (device == null)
                return new Response<DeviceReadDto>(HttpStatusCode.NotFound, "Device cannot be found");

            device.DeviceName = dto.DeviceName;
            device.TemplateId = dto.TemplateId;
            device.DeviceLocation = dto.DeviceLocation;

            var updated = await deviceRepository.UpdateAsync(device);

            if (!updated)
            {
                return new Response<DeviceReadDto>(HttpStatusCode.BadRequest, "Device Updation Failed.");
            }

            return new Response<DeviceReadDto>(HttpStatusCode.OK, device.MapToDto(), ResponseCode.Data);
        }

        public async Task<Response> DeleteDeviceAsync(string id)
        {
            var device = await deviceRepository.GetByIdAsync(id);
            if (device == null)
                return new Response(HttpStatusCode.NotFound, "Device cannot be found");

            var success = await deviceRepository.DeleteAsync(device);
            if (!success)
                return new Response(HttpStatusCode.BadRequest, "Device Delete Failed");

            return new Response(HttpStatusCode.OK, "Device Deleted Successfully");
        }

        public async Task<Response<IList<DeviceReadDto>>> GetAllDevicesAsync(string CharityId)
        {
            var devices = await deviceRepository.GetAllByCharityIdAsync(CharityId);

            var mapped = devices.Select(d => d.MapToDto()).ToList();

            return new Response<IList<DeviceReadDto>>(HttpStatusCode.OK, mapped, ResponseCode.Data);
        }

        public async Task<Response<DeviceReadDto>> GetDeviceByIdAsync(string id)
        {
            var device = await deviceRepository.GetByIdAsync(id);

            if (device == null)
                return new Response<DeviceReadDto>(HttpStatusCode.BadRequest, "Device Details not found");

            return new Response<DeviceReadDto>(HttpStatusCode.OK, device.MapToDto(), ResponseCode.Data);
        }
    }
}
