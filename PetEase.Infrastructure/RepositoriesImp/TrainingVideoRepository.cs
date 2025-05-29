using PetEase.Core.Models;
using PetEase.Core.Repositories;
using PetEase.Infrastructure.Data;
namespace PetEase.Infrastructure.RepositoriesImp
{
    public class TrainingVideoRepository :GenericRepository<TrainingVideo> ,ITrainingVideoRepository
    {
        private readonly ApplicationDbContext _context;
        public TrainingVideoRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
       
    }
}
