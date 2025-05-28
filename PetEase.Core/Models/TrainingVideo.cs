using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
