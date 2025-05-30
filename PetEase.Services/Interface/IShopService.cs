using PetEase.Core.Models;

namespace PetEase.Services.Interface
{
    public interface IShopService
    {
        Task<(IEnumerable<Shop>, int)> GetShopAsync(string? searchTerm, int pageNumber, int pageSize);
        Task<Shop?> GetShopByIdAsync(int id);
        Task<Shop> CreateShopAsync(Shop shop);
        Task<bool> UpdateAsync(Shop shop);
        Task<bool> DeleteAsync(int id);
    }
}
