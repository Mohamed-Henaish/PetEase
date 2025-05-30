using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PetEase.Core.Models;
using PetEase.Infrastructure.Services;
using PetEase.Services.Interface;

namespace PetEase.Presentation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopController : ControllerBase
    {
        private readonly IShopService _shopService;
        public ShopController(IShopService shopService)
        {
            _shopService = shopService;
        }
        [HttpGet]
        public async Task<IActionResult> GetShop([FromQuery] string? searchTerm, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var (shops, totalCount) = await _shopService.GetShopAsync(searchTerm, pageNumber, pageSize);
            return Ok(new
            {
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Data = shops
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetShopById(int id)
        {
            var shop = await _shopService.GetShopByIdAsync(id);
            if (shop == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = $"Shop with ID {id} not found"
                });
            }

            return Ok(new
            {
                success = true,
                message = "Shop retrieved successfully",
                data = shop
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Shop shop)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Validation failed",
                    errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }

            var created = await _shopService.CreateShopAsync(shop);
            return CreatedAtAction(nameof(GetShopById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Shop shop)
        {
            if (id != shop.Id)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Shop ID mismatch"
                });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Validation failed",
                    errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }

            var updated = await _shopService.UpdateAsync(shop);
            if (!updated)
            {
                return NotFound(new
                {
                    success = false,
                    message = $"Shop with ID {id} not found"
                });
            }

            return Ok(new
            {
                success = true,
                message = "Shop updated successfully"
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _shopService.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound(new
                {
                    success = false,
                    message = $"Shop with ID {id} not found"
                });
            }

            return Ok(new
            {
                success = true,
                message = "Shop deleted successfully"
            });
        }
    }
}
