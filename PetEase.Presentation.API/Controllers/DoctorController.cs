using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetEase.Core.Models;
using PetEase.Services.Interface;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
namespace PetEase.Presentation.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        // GET api/doctor/search?searchTerm=abc&pageNumber=1&pageSize=10
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string? searchTerm, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var (doctors, totalCount) = await _doctorService.GetDoctorsAsync(searchTerm, pageNumber, pageSize);
            return Ok(new
            {
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Data = doctors
            });
        }

        // GET api/doctor/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDoctor(int id)
        {
            var doctor = await _doctorService.GetDoctorByIdAsync(id);
            if (doctor == null)
                return NotFound();

            return Ok(doctor);
        }

        // POST api/doctor
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateDoctor([FromBody] Doctor doctor)
        {
            var created = await _doctorService.CreateDoctorAsync(doctor);
            return CreatedAtAction(nameof(GetDoctor), new { id = created.Id }, created);
        }

        // PUT api/doctor/5
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDoctor(int id, [FromBody] Doctor doctor)
        {
            if (id != doctor.Id)
            {
                return NotFound(new
                {
                    success = false,
                    message = $"Doctor with ID {id} not found"
                });
            }

            var isCreated = await _doctorService.UpdateDoctorAsync(doctor);
            if (!isCreated)
            {
                return NotFound(new
                {
                    success = false,
                    message = $"Doctor with ID {id} not found"
                });
            }
            return Ok(new
            {
                success = true,
                message = "Doctor updated successfully"
            });
        }

        // DELETE api/doctor/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var isValid = await _doctorService.DeleteDoctorAsync(id);
            if (!isValid)
            {
                return NotFound(new
                {
                    success = false,
                    message = $"Doctor with ID {id} not found"
                });
            }
            return Ok(new
            {
                success = true,
                message = "Doctor deleted successfully"
            });
        }

        // POST api/doctor/reserve
        [HttpPost("reserve")]
        [Authorize]
        public async Task<IActionResult> ReserveDoctor([FromBody] DoctorRequest request)
        {
            try
            {
                var result = await _doctorService.ReserveDoctorAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // You can log the exception here if needed
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
