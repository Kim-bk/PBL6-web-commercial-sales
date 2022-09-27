using System;
using System.Collections.Generic;

namespace CommercialClothes.Models.DTOs
{
    public class ItemDTO
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int ShopId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public DateTime? DateCreated { get; set; }
        public string Description { get; set; }
        public string Size { get; set; }
        public int Quantity { get; set; }
        public List<ImageDTO> Images { get; set; }
    }
}
