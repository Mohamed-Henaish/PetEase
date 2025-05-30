using PetEase.Core.Models;
namespace PetEase.Services.Interface
{
    public interface IDoctorService
    {
        Task<DoctorRequest> ReserveDoctorAsync(DoctorRequest request);
        Task<(IEnumerable<Doctor> doctors, int totalCount)> GetDoctorsAsync(string? searchTerm, int pageNumber, int pageSize);
        Task<Doctor> GetDoctorByIdAsync(int id);
        Task<Doctor> CreateDoctorAsync(Doctor doctor);
        Task<bool> UpdateDoctorAsync(Doctor doctor);
        Task<bool> DeleteDoctorAsync(int id);
    }
}
