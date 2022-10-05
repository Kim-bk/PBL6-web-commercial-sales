using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommercialClothes.Models.DTOs.Responses;

namespace CommercialClothes.Models.DTOs.Responsese
{
    public class ShopDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ItemDTO> Items{ get; set;}
    }
}