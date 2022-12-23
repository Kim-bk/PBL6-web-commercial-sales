using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CommercialClothes.Models.DTOs.Requests
{
    public class MoreItemRequest
    {
        public int ShopId { get; set; }
        public int CategoryId { get; set; }
        public List<int> Items { get; set; }
    }
}