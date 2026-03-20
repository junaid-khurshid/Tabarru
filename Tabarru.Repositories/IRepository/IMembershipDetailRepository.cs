using Tabarru.Repositories.Models;

namespace Tabarru.Repositories.IRepository
{
    public interface IMembershipDetailRepository
    {
        Task<bool> AddAsync(MembershipDetail membershipDetail);
        Task<MembershipDetail> GetByIdAsync(string PaymentId);
        Task<bool> UpdateAsync(MembershipDetail membershipDetail);
    }
}
