using PetEase.Core.Repositories;
using PetEase.Core.Models;
using PetEase.Services.Interface;

namespace PetEase.Infrastructure.Services
{
    public class TrainingVideoService : ITrainingVideoService
    {
        private readonly IUnitOfWork _uow;

        public TrainingVideoService(IUnitOfWork uow)
            => _uow = uow;

        public async Task<TrainingVideo> GetByIdAsync(int id)
            => await _uow.TrainingVideoRepository
                         .GetFirstorDefaultAsync(v => v.Id == id);

        public async Task<IEnumerable<TrainingVideo>> GetAllAsync()
            => await _uow.TrainingVideoRepository.GetAllAsync();

        public async Task<TrainingVideo> CreateAsync(TrainingVideo video)
        {
            await _uow.TrainingVideoRepository.AddAsync(video);
            await _uow.CompleteAsync();
            return video;
        }

        public async Task UpdateAsync(TrainingVideo video)
        {
            await _uow.TrainingVideoRepository.UpdateAsync(video);
            await _uow.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            await _uow.TrainingVideoRepository.RemoveAsync(entity);
            await _uow.CompleteAsync();
        }
        public async Task<(IEnumerable<TrainingVideo>, int)> GetPagedVideosAsync(string? searchTerm, int pageNumber, int pageSize)
        {
            return await _uow.TrainingVideoRepository.GetPagedVideosAsync(searchTerm, pageNumber, pageSize);
        }
    }
}
