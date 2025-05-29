using PetEase.Core.Models;

namespace PetEase.Services.Interface
{
    public interface IShopService
    {
        Task<IEnumerable<Shop>> GetAllAsync();
        Task<Shop?> GetByIdAsync(int id);
        Task<bool> CreateAsync(Shop shop);
        Task<bool> UpdateAsync(Shop shop);
        Task<bool> DeleteAsync(int id);
    }
}
