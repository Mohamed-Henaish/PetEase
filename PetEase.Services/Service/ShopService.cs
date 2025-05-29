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
        public async Task<IEnumerable<Shop>> GetAllAsync()
        {
            return await _unitOfWork.ShopRepository.GetAllAsync();
        }

        public async Task<Shop?> GetByIdAsync(int id)
        {
            return await _unitOfWork.ShopRepository.GetFirstorDefaultAsync(X => X.Id == id);
        }

        public async Task<bool> CreateAsync(Shop shop)
        {
            await _unitOfWork.ShopRepository.AddAsync(shop);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(Shop shop)
        {
            var existing = await _unitOfWork.ShopRepository.GetFirstorDefaultAsync(X => X.Id == shop.Id);
            if (existing == null) return false;

            _unitOfWork.ShopRepository.Update(shop);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            //var shop = await _unitOfWork.ShopRepository.GetFirstorDefaultAsync(X => X.Id == id);
            //if (shop == null) return false;

            //_unitOfWork.ShopRepository.RemoveRangeAsync(shop);
            //await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}
