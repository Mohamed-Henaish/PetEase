using Microsoft.EntityFrameworkCore;
using PetEase.Core.DTOs;
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
        public void Update(ShopDto shopDto)
        {
            var shopInDb = _context.TbShop.FirstOrDefault(x => x.Id == shopDto.Id);
            if (shopInDb != null)
            {
                shopInDb.Name = shopDto.Name;
                shopInDb.LogoUrl = shopDto.LogoUrl;
                shopInDb.ContactEmail = shopDto.ContactEmail;
                shopInDb.ContactPhone = shopDto.ContactPhone;
                shopInDb.Description = shopDto.Description;
                shopInDb.IsActive = shopDto.IsActive;
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
