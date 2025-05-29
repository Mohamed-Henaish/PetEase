

namespace PetEase.Core.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository ProductRepository { get; }
        IDoctorRepository DoctorRepository { get; }
        IOrderDetailsRepository OrderDetailsRepository { get; }
        IOrderHeaderRepository OrderHeaderRepository { get; }
        IShopRepository ShopRepository { get; }
        Task CompleteAsync();
    }
}
