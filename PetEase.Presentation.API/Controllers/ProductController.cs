using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    }
}
