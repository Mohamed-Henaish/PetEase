using PetEase.Core.DTOs;
using PetEase.Core.Models;


namespace PetEase.Core.Repositories
{
    public interface IShopRepository : IGenericRepository<Shop>
    {
        void Update(ShopDto shopDto);
        Task<(IEnumerable<Shop>, int)> GetPagedShopAsync(string? searchTerm, int pageNumber, int pageSize);
    }
}
