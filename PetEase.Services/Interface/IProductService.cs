using PetEase.Core.DTOs;
using PetEase.Core.Models;

namespace PetEase.Services.Interface
{
    public interface IProductService
    {
        Task<Product> CreateProductAsync(ProductDto productDto);
        Task<(IEnumerable<Product>, int)> GetProductAsync(string? searchTerm, int pageNumber, int pageSize);
        Task<Product?> GetProductByIdAsync(int id);
        Task<bool> UpdateProductAsync(int id, ProductDto productDto);
        Task<bool> DeleteProductAsync(int id);
    }
}
