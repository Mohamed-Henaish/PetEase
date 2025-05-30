using PetEase.Core.DTOs;
using PetEase.Core.Models;

namespace PetEase.Core.Repositories
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        void Update(ProductDto dto);
        Task<(IEnumerable<Product>, int)> GetPagedProductAsync(string? searchTerm, int pageNumber, int pageSize);
    }
}
