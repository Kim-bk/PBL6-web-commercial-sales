using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CommercialClothes.Models.DTOs.Responses
{
    public class ItemDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public int ShopId { get; set; }
        public List<ImageDTO> Images{ get; set;}
        public string Size { get; set; }
        public int Quantity { get; set; }
        public int CategoryId { get; set; }
        public DateTime? DateCreated { get; set; }
        public string Shop {get;set;}
    }
}