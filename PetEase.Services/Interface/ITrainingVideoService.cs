using PetEase.Core.Models;
namespace PetEase.Services.Interface
{
    public interface ITrainingVideoService
    {
        Task<TrainingVideo> GetByIdAsync(int id);
        Task<IEnumerable<TrainingVideo>> GetAllAsync();
        Task<TrainingVideo> CreateAsync(TrainingVideo video);
        Task<bool> UpdateAsync(TrainingVideo video);
        Task<bool> DeleteAsync(int id);
        Task<(IEnumerable<TrainingVideo> Videos, int TotalCount)> GetPagedVideosAsync(string? searchTerm, int pageNumber, int pageSize);
    }


}
