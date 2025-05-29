using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetEase.Core.Models
{
    public class DoctorRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int DoctorId { get; set; }


        [Required]
        [MaxLength(255)]
        public string UserName { get; set; }

        [Required]
        [Phone]
        [MaxLength(20)]
        public string UserPhone { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string UserEmail { get; set; }

        [Required]
        public DateTime ReservedTime { get; set; }

        public bool IsConfirmed { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
