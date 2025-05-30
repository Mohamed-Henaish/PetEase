using PetEase.Core.DTOs;
using PetEase.Core.Models;
using PetEase.Core.Repositories;
using PetEase.Services.Interface;

namespace PetEase.Services.Service
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<(IEnumerable<Product>, int)> GetProductAsync(string? searchTerm, int pageNumber, int pageSize)
        {
            return await _unitOfWork.ProductRepository.GetPagedProductAsync(searchTerm, pageNumber, pageSize);
        }
        public async Task<Product> CreateProductAsync(ProductDto productDto)
        {
            if (productDto == null)
                throw new ArgumentNullException(nameof(productDto));
            var product = new Product
            {
                Name = productDto.Name,
                MainImageUrl = productDto.MainImageUrl,
                Description = productDto.Description,
                StockQuantity = productDto.StockQuantity,
                Price = productDto.Price,
                DiscountPrice = productDto.DiscountPrice,
                ShopId = productDto.ShopId,
                ProductType = productDto.ProductType,
                AnimalType = productDto.AnimalType
            };
            await _unitOfWork.ProductRepository.AddAsync(product);
            await _unitOfWork.CompleteAsync();

            return product;
        }
        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await GetbyId(id);
        }
        public async Task<bool> UpdateProductAsync(int id, ProductDto dto)
        {
            var product = await GetbyId(id);
            if (product == null) return false;
            _unitOfWork.ProductRepository.Update(dto);
            await _unitOfWork.CompleteAsync();
            return true;
        }
        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await GetbyId(id);
            if (product == null) return false;

            await _unitOfWork.ProductRepository.RemoveAsync(product);
            await _unitOfWork.CompleteAsync();
            return true;
        }
        private async Task<Product> GetbyId(int id)
        {
            return await _unitOfWork.ProductRepository.GetFirstorDefaultAsync(X => X.Id == id);
        }
    }
}
