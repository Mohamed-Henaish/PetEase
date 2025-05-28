using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PetEase.Core.Models
{
    public class Address
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(500)]
        public string Location { get; set; } = string.Empty;
        [Required]
        [MaxLength(100)]
        public string City { get; set; } = string.Empty;
        [Required]
        [MaxLength(100)]
        public string State { get; set; } = string.Empty;
        [Required]
        [MaxLength(100)]
        public string Country { get; set; } = string.Empty;
        [ForeignKey("Doctor")]
        public int DoctorId { get; set; }
        [JsonIgnore]
        public virtual Doctor? Doctor { get; set; }
    }
}
