using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabarru.Common.Models
{
    public class LoginResponse
    {
        public LoginResponse()
        {

        }

        public LoginResponse(string accessToken, int expiry, string refreshToken)
        {
            AccessToken = accessToken;
            ExpiresIn = DateTime.UtcNow.AddSeconds(expiry);
            RefreshToken = refreshToken;
            EmailConfirmed = true;
        }
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }
        public DateTime ExpiresIn { get; set; }
        public bool EmailConfirmed { get; set; }
    }
}
