using Microsoft.AspNetCore.Mvc;
using PetEase.Core.Models;
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
        public async Task<IActionResult> GetAll()
        {
            var shops = await _shopService.GetAllAsync();
            return Ok(new
            {
                success = true,
                message = "Shops retrieved successfully",
                data = shops
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var shop = await _shopService.GetByIdAsync(id);
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

            var isCreated = await _shopService.CreateAsync(shop);
            if (!isCreated)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Failed to create shop"
                });
            }

            return Ok(new
            {
                success = true,
                message = "Shop created successfully",
                data = shop
            });
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
