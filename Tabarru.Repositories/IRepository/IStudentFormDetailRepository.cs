using Tabarru.Repositories.Models;

namespace Tabarru.Repositories.IRepository
{
    public interface IStudentFormDetailRepository
    {
        Task<bool> AddAsync(StudentFormDetail studentFormDetail);
        Task<StudentFormDetail> GetByIdAsync(string PaymentId);
        Task<bool> UpdateAsync(StudentFormDetail giftAidDetail);
    }
}
