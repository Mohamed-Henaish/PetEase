using PetEase.Core.Repositories;
using PetEase.Infrastructure.Data;

namespace PetEase.Infrastructure.RepositoriesImp
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _;

        public UnitOfWork(ApplicationDbContext context)
        {
            _ = context;
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
