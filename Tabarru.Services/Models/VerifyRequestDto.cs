namespace Tabarru.Services.Models
{
    public class VerifyRequestDto
    {
        public string Email { get; set; } = null!;
        public string Token { get; set; } = null!;
    }
}
