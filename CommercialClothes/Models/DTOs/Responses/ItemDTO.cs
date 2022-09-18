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
        // public int ShopId { get; set; }
        public List<ImageDTO> Images{ get; set;}
    }
}