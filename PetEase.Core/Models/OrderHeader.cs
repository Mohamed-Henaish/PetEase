using PetEase.Core.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace PetEase.Core.Models
{
    public class OrderHeader
    {
        [Key]
        public int Id { get; set; }
        public decimal SubTotal { get; set; }
        public decimal shippingCost { get; set; }
        public decimal totalPrice { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public string? CarrierName { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;

        // User Info
        [Required,MaxLength(450)]
        public string Name { get; set; }
        [Required,EmailAddress]
        public string Email { get; set; }
        [Required,MaxLength(1000)]
        public string Address { get; set; }
        [Required,MaxLength(600)]
        public string Government { get; set; }
        [Required,MaxLength(600)]
        public string City { get; set; }
        public int? PostalCode { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public string? AdditionalPhoneNumber { get; set; }


    }
}
