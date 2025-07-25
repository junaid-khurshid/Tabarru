using Tabarru.Services.Models;

namespace Tabarru.RequestModels
{
    public class LoginRequest
    {
        public string Email { get; set; }
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
