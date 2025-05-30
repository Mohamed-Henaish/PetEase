using Microsoft.EntityFrameworkCore;
using PetEase.Core.DTOs;
using PetEase.Core.Models;
using PetEase.Core.Repositories;
using PetEase.Infrastructure.Data;

namespace PetEase.Infrastructure.RepositoriesImp
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _context;
        protected readonly DbSet<Product> _dbSet;
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
            _dbSet = context.Set<Product>();
        }
        public void Update(ProductDto dto)
        {
            var productInDb = _context.TbProduct.FirstOrDefault(X => X.Id == dto.Id);
            if (productInDb != null)
            {
                productInDb.Name = dto.Name;
                productInDb.MainImageUrl = dto.MainImageUrl;
                productInDb.Description = dto.Description;
                productInDb.StockQuantity = dto.StockQuantity;
                productInDb.Price = dto.Price;
                productInDb.DiscountPrice = dto.DiscountPrice;
                productInDb.ShopId = dto.ShopId;
                productInDb.ProductType = dto.ProductType;
                productInDb.AnimalType = dto.AnimalType;
                productInDb.LastUpdated = DateTime.Now;
            }
        }

        public async Task<(IEnumerable<Product>, int)> GetPagedProductAsync(string? searchTerm, int pageNumber, int pageSize)
        {
            IQueryable<Product> query = _dbSet;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(d => d.Name.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync();

            var products = await query
                .OrderBy(d => d.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (products, totalCount);
        }
    }
}
