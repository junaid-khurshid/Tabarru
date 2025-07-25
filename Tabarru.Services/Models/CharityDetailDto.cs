using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabarru.Services.Models
{
    public class CharityDetailDto
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public bool KycStatus { get; set; }

        public bool EmailVerified { get; set; }

        public string Role { get; set; }
    }
}
