using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommercialClothes.Models.DTOs
{
    public class OrderDetailDTO
    {
        public int OrderDetailId { get; set; }
        // public int OrderId { get; set; }
        public string Size { get; set; }
        public string ItemName { get; set; }
        public int QuantityOrderDetail { get; set; }
        public int ItemId { get; set; }
        public string ItemImg { get; set; }
        public int Price { get; set; }

    }
}