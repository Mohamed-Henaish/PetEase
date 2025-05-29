using System.ComponentModel.DataAnnotations;

namespace PetEase.Core.Models
{
    public class TrainingVideo
    {
        [Key]
        public int Id { get; set; }
        [Required,MaxLength(450)]
        public string Title { get; set; }
        [Required]
        public string Link { get; set; } 
        [Required]
        public string Description { get; set; }
    }
}
