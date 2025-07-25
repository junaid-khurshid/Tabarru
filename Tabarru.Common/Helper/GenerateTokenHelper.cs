using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Tabarru.Common.Helper
{
    public static class GenerateTokenHelper
    {

        public static (string,DateTime) CreateToken(IConfiguration configuration, string Id, string EmailAddress, string Role)
        {
            var expiryTime = DateTime.Now.AddHours(1);
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, Id),
            new Claim(ClaimTypes.Email, EmailAddress),
            new Claim(ClaimTypes.Role, Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: expiryTime,
                signingCredentials: creds);

            return (new JwtSecurityTokenHandler().WriteToken(token), expiryTime);
        }

        public static (string, DateTime) GenerateRefreshToken()
        {
            var randomBytes = RandomNumberGenerator.GetBytes(64);
            var Expires = DateTime.UtcNow.AddDays(1);
            return (Convert.ToBase64String(randomBytes), Expires);
        }
    }
}
