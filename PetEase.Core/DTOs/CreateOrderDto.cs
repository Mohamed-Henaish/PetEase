using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetEase.Core.DTOs
{
    public class CreateOrderDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Government { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public int? PostalCode { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string? AdditionalPhoneNumber { get; set; }
        public string? Notes { get; set; }
        public string CarrierName { get; set; } = string.Empty;
        public decimal ShippingCost { get; set; }
        public List<OrderProductDto> Products { get; set; } = new List<OrderProductDto>();
    }

}
