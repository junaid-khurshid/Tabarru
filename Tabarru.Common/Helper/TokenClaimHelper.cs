using System.Security.Claims;

namespace Tabarru.Common.Helper
{
    public static class TokenClaimHelper
    {
        public static string? GetId(ClaimsPrincipal user)
        {
            return user?.FindFirst(ClaimTypes.Name)?.Value;
        }

        public static string? GetEmail(ClaimsPrincipal user)
        {
            return user?.FindFirst(ClaimTypes.Email)?.Value;
        }

        public static string? GetRole(ClaimsPrincipal user)
        {
            return user?.FindFirst(ClaimTypes.Role)?.Value;
        }

        /// <summary>
        /// Generic claim getter
        /// </summary>
        public static T? GetClaimValue<T>(ClaimsPrincipal user, string claimType)
        {
            var value = user?.FindFirst(claimType)?.Value;
            if (value == null)
                return default;

            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
