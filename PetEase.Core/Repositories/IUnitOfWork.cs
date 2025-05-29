

using PetEase.Core.Models;

namespace PetEase.Core.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository ProductRepository { get; }
        IDoctorRepository DoctorRepository { get; }
        IOrderDetailsRepository OrderDetailsRepository { get; }
        IOrderHeaderRepository OrderHeaderRepository { get; }
        IShopRepository ShopRepository { get; }
        ITrainingVideoRepository TrainingVideoRepository { get; }
        IDoctorRequestRepository DoctorRequestRepository { get; }

        Task CompleteAsync();
    }
}
    