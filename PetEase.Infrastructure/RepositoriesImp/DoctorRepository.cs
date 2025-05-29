using Microsoft.EntityFrameworkCore;
using PetEase.Core.Models;
using PetEase.Core.Repositories;
using PetEase.Infrastructure.Data;

namespace PetEase.Infrastructure.RepositoriesImp
{
    public class DoctorRepository : GenericRepository<Doctor>, IDoctorRepository
    {
        private readonly ApplicationDbContext _context;
        protected readonly DbSet<Doctor> _dbSet;

        public DoctorRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
            _dbSet = _context.Set<Doctor>();
        }

        public async Task<(IEnumerable<Doctor>, int)> GetPagedDoctorsAsync(string? searchTerm, int pageNumber, int pageSize)
        {
            IQueryable<Doctor> query = _dbSet;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(d => d.Name.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync();

            var doctors = await query
                .OrderBy(d => d.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (doctors, totalCount);
        }
    }
}
