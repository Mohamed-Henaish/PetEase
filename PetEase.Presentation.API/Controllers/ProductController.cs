using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetEase.Core.DTOs;
using PetEase.Core.Repositories;
using PetEase.Services.Interface;

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
    }
}
