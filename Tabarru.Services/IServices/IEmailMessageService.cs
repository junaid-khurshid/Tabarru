namespace Tabarru.Services.IServices
{
    public interface IEmailMessageService
    {
        Task<bool> SendEmailAsync(string toEmail, string subject, string htmlBody);
    }
}
