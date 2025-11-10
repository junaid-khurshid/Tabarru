using Tabarru.Common.Models;
using Tabarru.Services.Models;

namespace Tabarru.Services.IServices
{
    public interface IPaymentService
    {
        Task<Response> SavePayment(PaymentDto dto);

        Task<Response<List<PaymentReadDetailDto>>> GetByCharityIdAsync(string charityId);

        Task<Response<List<PaymentReadDetailDto>>> GetByCampaignOrTemplateIdAsync(string? campaignId, string? templateId);
    }
}
