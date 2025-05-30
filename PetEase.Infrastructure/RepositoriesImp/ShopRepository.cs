using Microsoft.EntityFrameworkCore;
using PetEase.Core.Models;
using PetEase.Core.Repositories;
using PetEase.Infrastructure.Data;

namespace PetEase.Infrastructure.RepositoriesImp
{
    public class ShopRepository : GenericRepository<Shop>, IShopRepository
    {
        private readonly ApplicationDbContext _context;
        protected readonly DbSet<Shop> _dbSet;
        public ShopRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
            _dbSet = _context.Set<Shop>();
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

        public async Task<(IEnumerable<Shop>, int)> GetPagedShopAsync(string? searchTerm, int pageNumber, int pageSize)
        {
            IQueryable<Shop> query = _dbSet;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(d => d.Name.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync();

            var shops = await query
                .OrderBy(d => d.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (shops, totalCount);
        }
    }
}
