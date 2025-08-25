using System.ComponentModel.DataAnnotations;
using Tabarru.Services.Models;

namespace Tabarru.RequestModels
{
    public class DeviceCreateRequest
    {
        [Required(ErrorMessage = "REQUIRED")]
        public string DeviceName { get; set; }

        [Required(ErrorMessage = "REQUIRED")] 
        public string TemplateId { get; set; }

        [Required(ErrorMessage = "REQUIRED")] 
        public string DeviceLocation { get; set; }
    }

    static class DeviceCreateExtension
    {
        public static DeviceDto MaptoDto(this DeviceCreateRequest deviceCreateRequest, string CharityId)
        {
            return new DeviceDto
            {
                DeviceName = deviceCreateRequest.DeviceName,
                CharityId = CharityId,
                DeviceLocation = deviceCreateRequest.DeviceLocation,
                TemplateId = deviceCreateRequest.TemplateId
            };
        }
    }
}
