using Microsoft.AspNetCore.Mvc;
using PetEase.Core.Models;
using PetEase.Services.Interface;
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

        [HttpPost]
        public async Task<IActionResult> Create(TrainingVideo video)
        {
            var created = await _svc.CreateAsync(video);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, TrainingVideo video)
        {
            if (id != video.Id) return BadRequest();
            await _svc.UpdateAsync(video);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _svc.DeleteAsync(id);
            return NoContent();
        }
    }
}
