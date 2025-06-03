using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PetEase.Core.DTOs;
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
        [HttpGet("search")]
        public async Task<IActionResult> GetShop(
                [FromQuery] string? searchTerm,
                [FromQuery] int pageNumber = 1,
                [FromQuery] int pageSize = 10)
        {
            var (shops, totalCount) = await _shopService.GetShopAsync(searchTerm, pageNumber, pageSize);
            var data = shops.Select(shop => new
            {
                shop.Id,
                shop.Name,
                shop.LogoUrl,
                shop.ContactEmail,
                shop.ContactPhone,
                shop.Description,
                shop.CreatedAt,
                shop.LastUpdated,
                shop.IsActive
            });

            return Ok(new
            {
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Data = data
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
                data = new
                {
                    shop.Id,
                    shop.Name,
                    shop.LogoUrl,
                    shop.ContactEmail,
                    shop.ContactPhone,
                    shop.Description,
                    shop.CreatedAt,
                    shop.LastUpdated,
                    shop.IsActive
                }
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ShopDto shopDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdShop = await _shopService.CreateShopAsync(shopDto);
            return Ok(new
            {
                message = "Shop created successfully",
                data = new
                {
                    createdShop.Id,
                    createdShop.Name,
                    createdShop.LogoUrl,
                    createdShop.ContactEmail,
                    createdShop.ContactPhone,
                    createdShop.Description,
                    createdShop.CreatedAt,
                    createdShop.IsActive
                }
            });
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ShopDto shopDto)
        {
            if (id != shopDto.Id)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Shop ID mismatch"
                });
            }
            var updated = await _shopService.UpdateAsync(shopDto);
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
        [Authorize(Roles = "Admin")]
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
