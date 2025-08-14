using Microsoft.AspNetCore.Http;

namespace Tabarru.Common.Helper
{
    public static class ConvertFileToBase64Helper
    {

        public async static Task<string> ConvertFileToBase64Async(this IFormFile file)
        {
            if (file == null || file.Length == 0)
                return string.Empty;

            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            var fileBytes = ms.ToArray();
            return Convert.ToBase64String(fileBytes);
        }
    }
}
