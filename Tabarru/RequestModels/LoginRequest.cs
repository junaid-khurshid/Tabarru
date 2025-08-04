using System.ComponentModel.DataAnnotations;
using Tabarru.Services.Models;

namespace Tabarru.RequestModels
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "REQUIRED"), EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "REQUIRED")]
        public string Password { get; set; }
    }

    static class LoginDetailExtension
    {
        public static LoginDto MapToDto(this LoginRequest loginRequest)
        {
            return new LoginDto
            {
                Email = loginRequest.Email,
                Password = loginRequest.Password,
            };
        }

    }
}
