using PetEase.Core.Repositories;
using PetEase.Infrastructure.Data;

namespace PetEase.Infrastructure.RepositoriesImp
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _;
        public IProductRepository ProductRepository { get; private set; }
        public IDoctorRepository DoctorRepository { get; private set; }
        public IOrderDetailsRepository OrderDetailsRepository { get; private set; }
        public IOrderHeaderRepository OrderHeaderRepository { get; private set; }
        public IShopRepository ShopRepository { get; private set; }
        public UnitOfWork(ApplicationDbContext context)
        {
            _ = context;
            ProductRepository = new ProductRepository(context);
            DoctorRepository = new DoctorRepository(context);
            OrderDetailsRepository = new OrderDetailsRepository(context);
            ShopRepository = new ShopRepository(context);
            OrderHeaderRepository = new OrderHeaderRepository(context);
        }
        public Task CompleteAsync()
        {
            return _.SaveChangesAsync();
        }

        public void Dispose()
        {
            _.Dispose();
        }
    }
}
