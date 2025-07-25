using System.ComponentModel.DataAnnotations;
using Tabarru.Services.Models;

namespace Tabarru.RequestModels
{
    public class RegisterCharityDetail
    {
        [Required(ErrorMessage = "REQUIRED"), EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "REQUIRED")]
        public string Password { get; set; }
       
        [Required(ErrorMessage = "REQUIRED")]
        public string Role { get; set; }

    }

    static class CharityDetailExtension
    {
        public static CharityDetailDto MapToDto(this RegisterCharityDetail userDetail)
        {
            return new CharityDetailDto
            {
                Email = userDetail.Email,
                Password = userDetail.Password,
                Role = userDetail.Role
            };
        }

    }
}
