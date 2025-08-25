using Tabarru.Repositories.Models;

namespace Tabarru.Repositories.IRepository
{
    public interface IDeviceRepository
    {
        Task<bool> AddAsync(Device device);
        Task<bool> UpdateAsync(Device device);
        Task<IEnumerable<Device>> GetAllByCharityIdAsync(string charityId);
        Task<Device> GetByIdAsync(string id);
        Task<bool> AnyByIdAsync(string id);
        Task<bool> DeleteAsync(Device device);
    }
}
