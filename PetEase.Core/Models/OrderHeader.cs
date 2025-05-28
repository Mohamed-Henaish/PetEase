using PetEase.Core.Models.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PetEase.Core.Models
{
    public class OrderHeader
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Subtotal must be positive")]
        public decimal SubTotal { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Shipping cost must be positive")]
        public decimal ShippingCost { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Total price must be positive")]
        public decimal TotalPrice { get; set; }

        [Required]
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        [MaxLength(450)]
        public string? CarrierName { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [DataType(DataType.DateTime)]
        public DateTime? ShippingDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DeliveryDate { get; set; }

        // User Info
        [Required]
        [MaxLength(450)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(1000)]
        public string Address { get; set; } = string.Empty;

        [Required]
        [MaxLength(600)]
        public string Government { get; set; } = string.Empty;

        [Required]
        [MaxLength(600)]
        public string City { get; set; } = string.Empty;

        [Range(0, 999999)]
        public int? PostalCode { get; set; }

        [Required]
        [Phone]
        [MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Phone]
        [MaxLength(20)]
        public string? AdditionalPhoneNumber { get; set; }

        [MaxLength(2000)]
        public string? Notes { get; set; }

        // Navigation property
        [JsonIgnore]
        public virtual ICollection<OrderDetails> OrderDetails { get; set; } = new List<OrderDetails>();
    }
}
