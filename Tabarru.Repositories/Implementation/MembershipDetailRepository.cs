using Microsoft.EntityFrameworkCore;
using Tabarru.Repositories.DatabaseContext;
using Tabarru.Repositories.IRepository;
using Tabarru.Repositories.Models;

namespace Tabarru.Repositories.Implementation
{
    public class MembershipDetailRepository : IMembershipDetailRepository
    {
        private readonly DbStorageContext dbStorageContext;

        public MembershipDetailRepository(DbStorageContext dbStorageContext)
        {
            this.dbStorageContext = dbStorageContext;
        }
        public async Task<bool> AddAsync(MembershipDetail membershipDetail)
        {
            dbStorageContext.MembershipDetails.Add(membershipDetail);
            return await dbStorageContext.SaveChangesAsync() > 0;
        }

        public async Task<MembershipDetail> GetByIdAsync(string PaymentId)
        {
            return await dbStorageContext.MembershipDetails.FirstOrDefaultAsync(x => x.Id.Equals(PaymentId));
        }

        public async Task<bool> UpdateAsync(MembershipDetail membershipDetail)
        {
            dbStorageContext.MembershipDetails.Update(membershipDetail);
            return await dbStorageContext.SaveChangesAsync() > 0;
        }
    }
}
