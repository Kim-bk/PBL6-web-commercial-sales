using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CommercialClothes.Models;

namespace CommercialClothes.Models.DTOs.Requests
{
    public class CartRequest
    {
        public int ShopId { get; set; }       
        public List<OrderDetailRequest> OrderDetails { get; set; }        
    }
}