using Tabarru.Repositories.Models;

namespace Tabarru.Repositories.IRepository
{
    public interface IGiftAidRepository
    {
        Task<bool> AddAsync(GiftAidDetail giftAidDetail);
        Task<GiftAidDetail> GetByIdAsync(string PaymentId);
        Task<bool> UpdateAsync(GiftAidDetail giftAidDetail);
    }
}
