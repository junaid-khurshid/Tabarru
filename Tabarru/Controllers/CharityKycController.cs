using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Tabarru.Common.Helper;
using Tabarru.Common.Models;
using Tabarru.RequestModels;
using Tabarru.Services.Implementation;
using Tabarru.Services.IServices;
using Tabarru.Services.Models;

namespace Tabarru.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "USER, ADMIN")]

    public class CharityKycController : ControllerBase
    {
        private readonly ICharityKycService kycService;
        private readonly ICharityAccountService charityAccountService;

        public CharityKycController(ICharityKycService kycService,
            ICharityAccountService charityAccountService)
        {
            this.kycService = kycService;
            this.charityAccountService = charityAccountService;
        }

        [HttpPost("submit")]
        public async Task<Response> SubmitKyc([FromForm] CharityKycSubmitRequest charityKycSubmitRequest)
        {
            return await kycService.SubmitKycAsync(TokenClaimHelper.GetId(User), charityKycSubmitRequest.MapToDto());
        }

        [HttpPut("re-submit")]
        public async Task<Response> UpdateKyc([FromForm] CharityKycReSubmitRequest charityKycReSubmitRequest)
        {
            return await kycService.SubmitKycAsync(charityKycReSubmitRequest.CharityId, charityKycReSubmitRequest.MapToDto());
        }

        [HttpPost("upload")]
        public async Task<Response<string>> Upload([FromForm] UploadFileRequest uploadFileRequest)
        {
            if (uploadFileRequest.File == null)
                return new Response<string>(HttpStatusCode.BadRequest, "File is required");

            return await kycService.UploadAsync(uploadFileRequest.File, uploadFileRequest.FileName, TokenClaimHelper.GetId(User));
        }


        [HttpGet("admin/all")]
        [Authorize(Roles = "ADMIN")]
        public async Task<Response<IList<CharityReadDto>>> GetAllCharities()
        {
            return await kycService.GetAllCharitiesForAdminAsync();
        }

        [HttpPut("admin/update-status")]
        [Authorize(Roles = "ADMIN")]
        public async Task<Response> UpdateKycStatus([FromBody] AdminKycUpdateDto dto)
        {
            return await kycService.UpdateKycStatusAsync(dto);
        }

        [HttpGet("kycImage")]
        public async Task<Response<string>> GetKycImage([FromQuery] string path)
        {
            return await kycService.GetKycImageAsync(path);
        }

        [HttpPut("update")]
        public async Task<Response> UpdateCharityDetails(UpdateCharityDetailsRequest request)
        {
            return await charityAccountService.UpdateCharityDetailsAsync(request.MapToDto(TokenClaimHelper.GetId(User)));
        }
    }
}
