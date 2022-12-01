using System;
using System.Collections.Generic;

namespace CommercialClothes.Models.DTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public int StatusId { get; set; }
        public int PaymentId { get; set; }
        public DateTime DateCreated { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; } 
        public string City { get; set; }
        public string Country { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }

    }
}