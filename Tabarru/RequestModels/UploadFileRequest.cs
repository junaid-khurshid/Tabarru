namespace Tabarru.RequestModels
{
    public class UploadFileRequest
    {
        public IFormFile File { get; set; }
        public string FileName { get; set; }
    }
}
