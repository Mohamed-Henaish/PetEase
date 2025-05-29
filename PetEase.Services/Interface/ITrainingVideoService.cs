using PetEase.Core.Models;
namespace PetEase.Services.Interface
{
    public interface ITrainingVideoService
    {
        Task<TrainingVideo> GetByIdAsync(int id);
        Task<IEnumerable<TrainingVideo>> GetAllAsync();
        Task<TrainingVideo> CreateAsync(TrainingVideo video);
        Task UpdateAsync(TrainingVideo video);
        Task DeleteAsync(int id);
    }


}
