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
        [Required]
        public int IdOrderDetail { get; set; }
        [Required]
        public int OrderId{ get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public int ItemId { get; set; }
        // [Required]
        // public Item Item { get; set; }
    }
}