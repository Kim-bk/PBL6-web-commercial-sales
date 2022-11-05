using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommercialClothes.Models.DTOs.Responses
{
    public class OrderResponse
    {
        public string ShopName {get; set;}
        public List<OrderDetailDTO> OrderDetails {get;set;}
        public int? Total { get; set; }
    }
}