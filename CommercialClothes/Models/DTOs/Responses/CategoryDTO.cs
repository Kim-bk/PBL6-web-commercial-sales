using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommercialClothes.Models.DTOs.Responses
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public int? ShopId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Gender { get; set; }   
        public virtual List<ItemDTO> Items { get; set; }
    }
}