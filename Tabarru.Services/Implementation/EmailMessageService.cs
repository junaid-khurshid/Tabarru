using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Tabarru.Services.IServices;

namespace Tabarru.Services.Implementation
{
    public class EmailMessageService : IEmailMessageService
    {
        private readonly IConfiguration _configuration;

        public EmailMessageService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> SendEmailAsync(string toEmail, string subject, string htmlBody)
        {
            var client = new MailjetClient(
                _configuration["Mailjet:ApiKey"],
                _configuration["Mailjet:SecretKey"]
            );

            var request = new MailjetRequest
            {
                Resource = SendV31.Resource
            }
            .Property(Send.Messages, new JArray {
            new JObject {
                {"From", new JObject {
                    {"Email", _configuration["Mailjet:FromEmail"]},
                    {"Name", _configuration["Mailjet:FromName"]}
                }},
                {"To", new JArray {
                    new JObject {
                        {"Email", toEmail},
                        {"Name", toEmail}
                    }
                }},
                {"Subject", subject},
                {"HTMLPart", htmlBody}
            }
            });

            var response = await client.PostAsync(request);
            return response.IsSuccessStatusCode;
        }
    }
}
