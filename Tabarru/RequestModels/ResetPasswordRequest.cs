using System.ComponentModel.DataAnnotations;
using Tabarru.Services.Models;

namespace Tabarru.RequestModels
{
    public class ResetPasswordRequest
    {
        [Required(ErrorMessage = "REQUIRED")]
        public string Token { get; set; }

        [Required(ErrorMessage = "REQUIRED")]
        public string NewPassword { get; set; }
    }

    public static class ResetPassworddExtension
    {
        public static ResetPasswordDto MapToDto(this ResetPasswordRequest request)
        {
            return new ResetPasswordDto
            {
                NewPassword = request.NewPassword,
                Token = request.Token
            };
        }
    }
}
