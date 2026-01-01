using Microsoft.AspNetCore.Http;

namespace Tabarru.Services.IServices
{
    public interface IFileStoringService
    {
        Task<string> UploadAsync(IFormFile file, string fileName, string folder);
        Task<string> GetAsync(string path);
    }
}
