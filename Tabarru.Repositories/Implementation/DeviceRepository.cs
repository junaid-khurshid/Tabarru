using Microsoft.EntityFrameworkCore;
using Tabarru.Repositories.DatabaseContext;
using Tabarru.Repositories.IRepository;
using Tabarru.Repositories.Models;

namespace Tabarru.Repositories.Implementation
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly DbStorageContext dbStorageContext;

        public DeviceRepository(DbStorageContext dbStorageContext)
        {
            this.dbStorageContext = dbStorageContext;
        }

        public async Task<bool> AddAsync(Device device)
        {
            dbStorageContext.Devices.Add(device);
            return await dbStorageContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(Device device)
        {
            dbStorageContext.Devices.Update(device);
            return await dbStorageContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Device device)
        {
            dbStorageContext.Devices.Remove(device);
            return await dbStorageContext.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Device>> GetAllByCharityIdAsync(string charityId)
        {
            return await dbStorageContext.Devices.Where(x => x.CharityId.Equals(charityId)).ToListAsync();
        }

        public async Task<Device> GetByIdAsync(string id)
        {
            return await dbStorageContext.Devices.FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<bool> AnyByIdAsync(string Id)
            => await dbStorageContext.Campaigns.AnyAsync(c => c.Id == Id);
    }
}
