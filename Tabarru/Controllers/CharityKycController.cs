using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Tabarru.Common.Helper;
using Tabarru.Common.Models;
using Tabarru.RequestModels;
using Tabarru.Services.IServices;
using Tabarru.Services.Models;

namespace Tabarru.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "USER, ADMIN")]

    public class CharityKycController : ControllerBase
    {
        private readonly ICharityKycService _kycService;

        public CharityKycController(ICharityKycService kycService)
        {
            _kycService = kycService;
        }

        [HttpPost("submit")]
        public async Task<Response> SubmitKyc([FromForm] CharityKycSubmitRequest dto)
        {
            return await _kycService.SubmitKycAsync(TokenClaimHelper.GetId(User), dto.MapToDto());
        }

        [HttpPut("re-submit")]
        public async Task<Response> UpdateKyc([FromForm] CharityKycSubmitRequest dto)
        {
            return await _kycService.SubmitKycAsync(TokenClaimHelper.GetId(User), dto.MapToDto());
        }

        [HttpPost("upload")]
        public async Task<Response<string>> Upload([FromForm] UploadFileRequest uploadFileRequest)
        {
            if (uploadFileRequest.File == null)
                return new Response<string>(HttpStatusCode.BadRequest, "File is required");

            return await _kycService.UploadAsync(uploadFileRequest.File, uploadFileRequest.FileName, TokenClaimHelper.GetId(User));
        }


        [HttpGet("admin/all")]
        public async Task<Response<IList<CharityReadDto>>> GetAllCharities()
        {
            return await _kycService.GetAllCharitiesForAdminAsync();
        }

        [HttpPut("admin/update-status")]
        public async Task<Response> UpdateKycStatus([FromBody] AdminKycUpdateDto dto)
        {
            return await _kycService.UpdateKycStatusAsync(dto);
        }

        [HttpGet("kycImage")]
        public async Task<Response<string>> GetKycImage([FromQuery] string path)
        {
            return await _kycService.GetKycImageAsync(path);
        }
    }
}
