using Tabarru.Repositories.Models;

namespace Tabarru.Services.Models
{
    public class DeviceDto
    {
        public string DeviceName { get; set; }
        public string CharityId { get; set; }
        public string TemplateId { get; set; }
        public string DeviceLocation { get; set; }
    }

    static class DeviceExtension
    {
        public static DeviceReadDto MapToDto(this Device devicedDto)
        {
            return new DeviceReadDto
            {
                Id = devicedDto.Id,
                CharityId = devicedDto.CharityId,
                DeviceName = devicedDto.DeviceName,
                DeviceLocation = devicedDto.DeviceLocation,
                TemplateId = devicedDto.TemplateId
            };
        }
    }

    public class DeviceReadDto
    {
        public string Id { get; set; }
        public string DeviceName { get; set; }
        public string CharityId { get; set; }
        public string TemplateId { get; set; }
        public string DeviceLocation { get; set; }
    }
}
