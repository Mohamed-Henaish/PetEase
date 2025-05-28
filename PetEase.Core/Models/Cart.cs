using System.ComponentModel.DataAnnotations;

namespace PetEase.Core.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
