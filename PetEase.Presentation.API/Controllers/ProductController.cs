using Microsoft.AspNetCore.Mvc;
using PetEase.Core.DTOs;
using PetEase.Services.Interface;
using PetEase.Services.Service;

namespace PetEase.Presentation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet("search")]
        public async Task<IActionResult> GetProduct(
        [FromQuery] string? searchTerm,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
        {
            var (products, totalCount) = await _productService.GetProductAsync(searchTerm, pageNumber, pageSize);
            return Ok(new
            {
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Data = products
            });
        }

        [HttpPost]
        public async Task<ActionResult> CreateProduct([FromBody] ProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Invalid request data",
                    errors = ModelState.Values.SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                });
            }
            var createdProduct = await _productService.CreateProductAsync(productDto);
            return Ok(new
            {
                success = true,
                message = "Product created successfully",
                data = new
                {
                    createdProduct.Id,
                    createdProduct.Name,
                    createdProduct.MainImageUrl,
                    createdProduct.Description,
                    createdProduct.StockQuantity,
                    createdProduct.Price,
                    createdProduct.DiscountPrice,
                    createdProduct.CreatedAt,
                    createdProduct.LastUpdated,
                    createdProduct.ShopId,
                    createdProduct.ProductType,
                    createdProduct.AnimalType
                }
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound(new { success = false, message = "Product not found" });

            return Ok(new
            {
                success = true,
                message = "Product retrieved successfully",
                data = product
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    success = false, 
                    message = "Invalid data",
                    errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });

            var isUpdated = await _productService.UpdateProductAsync(id, dto);
            if (!isUpdated)
                return NotFound(new
                {
                    success = false,
                    message = "Product not found"
                });

            return Ok(new
            {
                success = true,
                message = "Product updated successfully", 
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProductAsync(id);
            if (!result)
                return NotFound(new 
                {
                    success = false, 
                    message = "Product not found" 
                });

            return Ok(new 
            { 
                success = true, 
                message = "Product deleted successfully" 
            });
        }
    }
}
