using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetEase.Core.Models
{
    public class Shop
    {
        [Key]
        public int Id { get; set; }
        [Required,MaxLength(450)]
        public string Name { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [Required]
        public string contactEmail { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
