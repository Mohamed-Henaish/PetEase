using PetEase.Core.Models.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetEase.Core.Models
{
    public class OrderDetails
    {
        [Key]
        public int Id { get; set; }
        public int OrderHeaderId { get; set; }
        public OrderHeader? orderHeader { get; set; }

        public int productId { get; set; }
        public Product? product { get; set; }
        public int Quantity { get; set; }
        public int ShopId { get; set; }
        public Shop? shop { get; set; }
    }
}
