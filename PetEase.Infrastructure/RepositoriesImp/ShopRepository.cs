using PetEase.Core.Models;
using PetEase.Core.Repositories;
using PetEase.Infrastructure.Data;

namespace PetEase.Infrastructure.RepositoriesImp
{
    public class ShopRepository : GenericRepository<Shop>, IShopRepository
    {
        private readonly ApplicationDbContext _context;
        public ShopRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public void Update(Shop shop)
        {
            var shopInDb = _context.TbShop.FirstOrDefault(x => x.Id == shop.Id);
            if (shopInDb != null)
            {
                shopInDb.Name = shop.Name;
                shopInDb.LogoUrl = shop.LogoUrl;
                shopInDb.ContactEmail = shop.ContactEmail;
                shopInDb.ContactPhone = shop.ContactPhone;
                shopInDb.Description = shop.Description;
                shopInDb.IsActive = shop.IsActive;
                shopInDb.LastUpdated = DateTime.Now;
            }
        }
    }
}
