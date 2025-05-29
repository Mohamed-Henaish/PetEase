using PetEase.Core.Models;
using PetEase.Core.Repositories;
using PetEase.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetEase.Infrastructure.RepositoriesImp
{
    public class DoctorRepository : GenericRepository<Doctor> ,IDoctorRepository
    {
        private readonly ApplicationDbContext _context;
        public DoctorRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
