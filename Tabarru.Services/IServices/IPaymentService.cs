using Tabarru.Common.Models;
using Tabarru.Services.Models;

namespace Tabarru.Services.IServices
{
    public interface IPaymentService
    {
        Task<Response> SavePayment(PaymentDto dto);
    }
}
