using Microsoft.EntityFrameworkCore;
using PetEase.Core.DTOs;
using PetEase.Core.Models;
using PetEase.Core.Repositories;
using PetEase.Infrastructure.Data;
namespace PetEase.Infrastructure.RepositoriesImp
{
    public class TrainingVideoRepository : GenericRepository<TrainingVideo>, ITrainingVideoRepository
    {
        private readonly ApplicationDbContext _context;
        protected readonly DbSet<TrainingVideo> _dbSet;

        public TrainingVideoRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
            _dbSet = _context.Set<TrainingVideo>();
        }
        public void Update(TrainingVideo trainingVideo)
        {
            var videoInDb = _context.TbTrainingVideo.FirstOrDefault(x => x.Id == trainingVideo.Id);
            if (videoInDb != null)
            {
                videoInDb.Title = trainingVideo.Title;
                trainingVideo.Description = trainingVideo.Description;
                trainingVideo.Link = trainingVideo.Link;
            }
        }

        public async Task<(IEnumerable<TrainingVideo> Videos, int TotalCount)> GetPagedVideosAsync(string? searchTerm, int pageNumber, int pageSize)
        {
            IQueryable<TrainingVideo> query = _dbSet;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(v =>
                    v.Title.Contains(searchTerm) ||
                    v.Link.Contains(searchTerm)); 
            }

            var totalCount = await query.CountAsync();

            var videos = await query
                .OrderBy(v => v.Title)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (videos, totalCount);
        }
    }
}
