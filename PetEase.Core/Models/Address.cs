using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetEase.Core.Models
{
    public class Address
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Location { get; set; }
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
    }
}
