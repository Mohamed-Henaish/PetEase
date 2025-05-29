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
        // Create Product
        // Get all Products
        // Get Product by Id
        // Update Product
        // Delete Product
    }
}
