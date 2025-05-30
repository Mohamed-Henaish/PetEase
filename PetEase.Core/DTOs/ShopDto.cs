using System.ComponentModel.DataAnnotations;

namespace PetEase.Core.DTOs
{
    public class ShopDto
    {
        public int Id { get; set; }
        [Required, MaxLength(450)]
        public string Name { get; set; }

        [Required, Url, MaxLength(500)]
        public string LogoUrl { get; set; }

        [Required, EmailAddress, MaxLength(255)]
        public string ContactEmail { get; set; }

        [Phone, MaxLength(20)]
        public string? ContactPhone { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
