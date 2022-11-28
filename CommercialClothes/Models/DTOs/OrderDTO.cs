using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommercialClothes.Models.DTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public int StatusId { get; set; }
        public int PaymentId { get; set; }
        public DateTime DateCreated { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; } // thêm cả city, country
        public List<OrderDetailDTO> OrderDetailsDTO { get; set; }

    }
}