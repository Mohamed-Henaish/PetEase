using PetEase.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetEase.Core.Repositories
{
    public interface IDoctorRepository : IGenericRepository<Doctor>
    {
        Task<(IEnumerable<Doctor> Doctors, int TotalCount)> GetPagedDoctorsAsync(
        string? searchTerm,
        int pageNumber,
        int pageSize);
    }
}
