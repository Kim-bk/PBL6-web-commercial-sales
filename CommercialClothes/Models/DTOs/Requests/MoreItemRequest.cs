using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CommercialClothes.Models.DTOs.Requests
{
    public class MoreItemRequest
    {
        [Required]
        public int ShopId { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public List<int> Items { get; set; }
    }
}