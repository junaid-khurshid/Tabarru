using System.ComponentModel.DataAnnotations;
using Tabarru.Services.Models;

namespace Tabarru.RequestModels
{
    public class ForgotPasswordRequest
    {
        [Required(ErrorMessage = "REQUIRED")]

        public string Email { get; set; }
    }

    public static class ForgotPasswordExtension
    {
        public static ForgotPasswordDto MapToDto(this ForgotPasswordRequest request)
        {
            return new ForgotPasswordDto
            {
                Email = request.Email
            };
        }
    }
}
