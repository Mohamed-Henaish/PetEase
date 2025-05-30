using PetEase.Core.Models;


namespace PetEase.Core.Repositories
{
    public interface IShopRepository : IGenericRepository<Shop>
    {
        void Update(Shop shop);
        Task<(IEnumerable<Shop>, int)> GetPagedShopAsync(string? searchTerm, int pageNumber, int pageSize);
    }
}
