using Tabarru.Repositories.Models;

namespace Tabarru.Repositories.IRepository
{
    public interface IRecurringPaymentRepository
    {
        Task<bool> AddAsync(RecurringPaymentDetail recurringPaymentDetail);
        Task<RecurringPaymentDetail> GetByIdAsync(string Id);
        Task<bool> UpdateAsync(RecurringPaymentDetail recurringPaymentDetail);
    }
}
