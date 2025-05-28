using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetEase.Core.Models
{
    public class Shop
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required,MaxLength(450)]
        public string Name { get; set; }
        [Required]
        [Url]
        [MaxLength(500)]
        public string LogoUrl { get; set; }
        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string ContactEmail { get; set; } = string.Empty;
        [Phone]
        [MaxLength(20)]
        public string? ContactPhone { get; set; }
        [MaxLength(1000)]
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? LastUpdated { get; set; }
        [Required]
        public bool IsActive { get; set; } = true;
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
