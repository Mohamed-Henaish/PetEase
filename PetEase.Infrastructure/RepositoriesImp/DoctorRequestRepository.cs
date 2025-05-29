using PetEase.Core.Models;
using PetEase.Core.Repositories;
using PetEase.Infrastructure.Data;
namespace PetEase.Infrastructure.RepositoriesImp
{
    public class DoctorRequestRepository : GenericRepository<DoctorRequest> ,IDoctorRequestRepository
    {
        private readonly ApplicationDbContext _context;
        public DoctorRequestRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
