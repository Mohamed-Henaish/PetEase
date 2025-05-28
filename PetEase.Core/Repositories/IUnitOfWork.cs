

namespace PetEase.Core.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        Task CompleteAsync();
    }
}
