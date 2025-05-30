using PetEase.Core.DTOs;
using PetEase.Core.Models;
using PetEase.Core.Models.Enum;
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
        private async Task<Product> GetbyId(int id)
        {
            return await _unitOfWork.ProductRepository.GetFirstorDefaultAsync(X => X.Id == id);
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
        // Get all Products
        // Get Product by Id
        // Update Product
        // Delete Product
    }
}
