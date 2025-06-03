using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetEase.Core.DTOs;
using PetEase.Core.Models;
using PetEase.Services.Interface;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
namespace PetEase.Presentation.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrainingVideoController : ControllerBase
    {
        private readonly ITrainingVideoService _svc;
        public TrainingVideoController(ITrainingVideoService svc)
            => _svc = svc;

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _svc.GetAllAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var v = await _svc.GetByIdAsync(id);
            if (v == null) return NotFound();
            return Ok(v);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(TrainingVideo video)
        {
            var created = await _svc.CreateAsync(video);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, TrainingVideo video)
        {
            if (id != video.Id)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Video ID mismatch"
                });
            }
            var updated = await _svc.UpdateAsync(video);
            if (!updated)
            {
                return NotFound(new
                {
                    success = false,
                    message = $"Video with ID {id} not found"
                });
            }
            return Ok(new
            {
                success = true,
                message = "Video updated successfully"
            });
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _svc.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound(new
                {
                    success = false,
                    message = $"Video with ID {id} not found"
                });
            }

            return Ok(new
            {
                success = true,
                message = "Video deleted successfully"
            });
        }
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string? searchTerm, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var (videos, totalCount) = await _svc.GetPagedVideosAsync(searchTerm, pageNumber, pageSize);
            return Ok(new
            {
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Data = videos
            });
        }
    }
}
