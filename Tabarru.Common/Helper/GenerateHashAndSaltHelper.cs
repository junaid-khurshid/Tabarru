using System.Security.Cryptography;
using System.Text;

namespace Tabarru.Common.Helper
{
    public static class GenerateHashAndSaltHelper
    {

        public static (byte[], byte[]) CreatePasswordHash(string password)
        {
            using var hmac = new HMACSHA512();
            byte[] salt = hmac.Key;
            byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return (salt, hash);
        }

        public static bool IsValidStringHash(string password, byte[] hash, byte[] salt)
        {
            using var hmac = new HMACSHA512(salt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return hash.SequenceEqual(computedHash);
        }
    }
}
