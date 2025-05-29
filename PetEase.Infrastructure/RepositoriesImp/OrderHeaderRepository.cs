using PetEase.Core.Models;
using PetEase.Core.Repositories;
using PetEase.Infrastructure.Data;


namespace PetEase.Infrastructure.RepositoriesImp
{
    public class OrderHeaderRepository : GenericRepository<OrderHeader> , IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderHeaderRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
