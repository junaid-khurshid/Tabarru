using Microsoft.EntityFrameworkCore;
using Tabarru.Repositories.DatabaseContext;
using Tabarru.Repositories.IRepository;
using Tabarru.Repositories.Models;

namespace Tabarru.Repositories.Implementation
{
    public class StudentFormDetailRepository : IStudentFormDetailRepository
    {
        private readonly DbStorageContext dbStorageContext;

        public StudentFormDetailRepository(DbStorageContext dbStorageContext)
        {
            this.dbStorageContext = dbStorageContext;
        }
        public async Task<bool> AddAsync(StudentFormDetail studentFormDetail)
        {
            dbStorageContext.StudentFormDetails.Add(studentFormDetail);
            return await dbStorageContext.SaveChangesAsync() > 0;
        }

        public async Task<StudentFormDetail> GetByIdAsync(string PaymentId)
        {
            return await dbStorageContext.StudentFormDetails.FirstOrDefaultAsync(x => x.Id.Equals(PaymentId));
        }

        public async Task<bool> UpdateAsync(StudentFormDetail studentFormDetail)
        {
            dbStorageContext.StudentFormDetails.Update(studentFormDetail);
            return await dbStorageContext.SaveChangesAsync() > 0;
        }
    }
}
