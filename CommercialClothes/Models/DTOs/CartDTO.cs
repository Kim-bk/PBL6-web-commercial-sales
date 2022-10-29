using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommercialClothes.Models.DTOs
{
    public class CartDTO
    {
        public int AccountId { get; set; }
        public int OrderId {get; set;}
        public List<OrderDetailDTO> OrderDetails{ get; set; }
    }
}