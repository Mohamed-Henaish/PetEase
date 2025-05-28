using System.ComponentModel.DataAnnotations;
using System.Net;

namespace PetEase.Core.Models
{
    public class Doctor
    {
        [Key]
        public int Id { get; set; }
        [Required,MaxLength(450)]
        public string Name { get; set; }
        public Address Address { get; set; }
        public ICollection<TimeSlot> TimeSlots { get; set; }
        public decimal Price { get; set; }
    }
}
