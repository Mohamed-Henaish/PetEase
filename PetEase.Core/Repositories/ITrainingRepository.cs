using PetEase.Core.Models;
namespace PetEase.Core.Repositories
{
    public interface ITrainingVideoRepository : IGenericRepository<TrainingVideo>
    {
        void Update(TrainingVideo trainingVideo);
        Task<(IEnumerable<TrainingVideo> Videos, int TotalCount)> GetPagedVideosAsync(string? searchTerm, int pageNumber, int pageSize);

    }
}
