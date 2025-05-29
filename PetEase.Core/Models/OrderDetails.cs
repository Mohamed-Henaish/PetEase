using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PetEase.Core.Models
{
    public class OrderDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [ForeignKey("OrderHeader")]
        public int OrderHeaderId { get; set; }
        [JsonIgnore]
        public virtual OrderHeader? OrderHeader { get; set; }
        [Required]
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product? Product { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }
        [Required]
        public int ShopId { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be positive")]
        public decimal UnitPrice { get; set; } 
        [NotMapped]
        public decimal TotalUnitPrice => UnitPrice * Quantity;
    }
}
// --> UnitPrice
// Stores the product's price at the moment of purchase.
// Protects against future price changes (if the product's price changes later, the order history remains accurate)