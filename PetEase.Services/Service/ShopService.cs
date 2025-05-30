using PetEase.Core.Models;
using PetEase.Core.Repositories;
using PetEase.Services.Interface;

namespace PetEase.Services.Service
{
    public class ShopService : IShopService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShopService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<(IEnumerable<Shop>, int)> GetShopAsync(string? searchTerm, int pageNumber, int pageSize)
        {
            return await _unitOfWork.ShopRepository.GetPagedShopAsync(searchTerm, pageNumber, pageSize);
        }

        public async Task<Shop?> GetShopByIdAsync(int id)
        {
            return await _unitOfWork.ShopRepository.GetFirstorDefaultAsync(X => X.Id == id);
        }

        public async Task<Shop> CreateShopAsync(Shop shop)
        {
            await _unitOfWork.ShopRepository.AddAsync(shop);
            await _unitOfWork.CompleteAsync();
            return shop;
        }

        public async Task<bool> UpdateAsync(Shop shop)
        {
            var existing = await GetShopById(shop.Id);
            if (existing == null) return false;

            _unitOfWork.ShopRepository.Update(shop);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var shop = await GetShopById(id);
            if (shop == null) throw new Exception("Doctor not found");

            await _unitOfWork.ShopRepository.RemoveAsync(shop);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<Shop> GetShopById(int id)
        {
            return await _unitOfWork.ShopRepository.GetFirstorDefaultAsync(d => d.Id == id);
        }
    }
}
