using Tabarru.Common.Models;
using Tabarru.Services.Models;

namespace Tabarru.Services.IServices
{
    public interface ICharityAccountService
    {
        Task<Response> Register(CharityDetailDto dto);
        Task<CharityDetailDto> GetByEmail(string email);
        Task<Response<LoginResponse>> Login(LoginDto dto);
        Task<Response> ReGenerateEmailVerificationTokenByEmail(string email);
        Task<Response> VerifyToken(VerifyRequestDto request);
    }
}
