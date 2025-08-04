using System.ComponentModel.DataAnnotations;
using Tabarru.Services.Models;

namespace Tabarru.RequestModels
{
    public class VerifyRequest
    {
        [Required(ErrorMessage = "REQUIRED")]
        public string Email { get; set; }
        [Required(ErrorMessage = "REQUIRED")]
        public string Token { get; set; }
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
