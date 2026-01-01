using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Tabarru.Services.IServices;

namespace Tabarru.Services.Implementation
{
    public class FileStoringService : IFileStoringService
    {

        private readonly IAmazonS3 _s3;
        private const string BucketName = "tabarru-kyc";
        public FileStoringService(IAmazonS3 s3)
        {
            _s3 = s3;
        }

        public async Task<string> UploadAsync(IFormFile file, string fileName, string folder)
        {
            var key = $"{folder}/{fileName}/{Guid.NewGuid()}";

            using var stream = file.OpenReadStream();

            var request = new PutObjectRequest
            {
                BucketName = BucketName,
                Key = key,
                InputStream = stream,
                ContentType = file.ContentType,
                AutoCloseStream = true
            };

            await _s3.PutObjectAsync(request);
            return key;
        }

        public async Task<string> GetAsync(string key)
        {
            var response = await _s3.GetObjectAsync(BucketName, key);

            using var memory = new MemoryStream();
            await response.ResponseStream.CopyToAsync(memory);

            return Convert.ToBase64String(memory.ToArray());
        }
    }
}
