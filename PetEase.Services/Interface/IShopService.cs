using PetEase.Core.DTOs;
using PetEase.Core.Models;

namespace PetEase.Services.Interface
{
    public interface IShopService
    {
        Task<(IEnumerable<Shop>, int)> GetShopAsync(string? searchTerm, int pageNumber, int pageSize);
        Task<Shop?> GetShopByIdAsync(int id);
        Task<Shop> CreateShopAsync(ShopDto shopDto);
        Task<bool> UpdateAsync(ShopDto shopDto);
        Task<bool> DeleteAsync(int id);
    }
}
