using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabarru.Common.Helper
{
    public static class EmailMaskHelper
    {
        public static string MaskEmail(string email)
        {
            if (string.IsNullOrEmpty(email) || !email.Contains("@"))
                return email;

            var parts = email.Split('@');
            var name = parts[0];
            var domain = parts[1];

            if (name.Length <= 2)
                return $"{name[0]}*@{domain}";

            return $"{name.Substring(0, 1)}****{name.Substring(name.Length - 1)}@{domain}";
        }
    }
}
