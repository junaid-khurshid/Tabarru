using Tabarru.Services.Models;

namespace Tabarru.RequestModels
{
    public class VerifyRequest
    {
        public string Email { get; set; } = null!;
        public string Token { get; set; } = null!;
    }

    static class VerifyRequestExtension
    {
        public static VerifyRequestDto MapToDto(this VerifyRequest verifyRequest)
        {
            return new VerifyRequestDto
            {
                Email = verifyRequest.Email,
                Token = verifyRequest.Token,
            };
        }

    }
}
