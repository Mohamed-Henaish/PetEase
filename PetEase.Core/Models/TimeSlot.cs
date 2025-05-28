using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PetEase.Core.Models
{
    public class TimeSlot
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public DateTime EndTime { get; set; }
        [Required]
        public bool IsAvailable { get; set; } = true;
        [ForeignKey("Doctor")]
        public int DoctorId { get; set; }
        [JsonIgnore]
        public virtual Doctor? Doctor { get; set; }
    }
}
