using PetEase.Core.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace PetEase.Core.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        [Required, MaxLength(450)]
        public string Name { get; set; }
        [Required,Url]
        public string MainImageUrl { get; set; }
        [Required, MaxLength(1000)]
        public string Description { get; set; }
        public int StockQuantity { get; set; } = 0;
        [Required]
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        [Required]
        public int ShopId { get; set; }
        public ProductType ProductType { get; set; } = ProductType.Accsesories;
        public AnimalType AnimalType { get; set; } = AnimalType.Cat;
    }
}
