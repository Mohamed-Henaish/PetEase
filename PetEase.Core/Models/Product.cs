using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetEase.Core.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required,MaxLength(450)]
        public string Name { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int StockNumber { get; set; }
        [Required]
        public decimal Price { get; set; }
        public int ShopId { get; set; }
        public Shop Shop { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
    }
}
