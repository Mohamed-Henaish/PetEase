using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PetEase.Core.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required,MaxLength(450)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [Url]
        [MaxLength(500)]
        public string MainImageUrl { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string Description { get; set; } = string.Empty;
        [Required]
        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue)]
        public decimal? DiscountPrice { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? LastUpdated { get; set; }
        [Required]
        [ForeignKey("Shop")]
        public int ShopId { get; set; }
        [JsonIgnore]
        public virtual Shop? Shop { get; set; }
    }
}
