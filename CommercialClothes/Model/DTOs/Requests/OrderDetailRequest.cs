using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ComercialClothes.Models; 

namespace CommercialClothes.Models.DTOs.Requests
{
    public class OrderDetailRequest
    {
        public int IdOrderDetail { get; set; }
        public int OrderId{ get; set; }
        public int Quantity { get; set; }
        public int ItemId { get; set; }

        // [Required]
        // public Item Item { get; set; }
    }
}