using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ComercialClothes.Models;

namespace CommercialClothes.Models.DTOs.Requests
{
    public class ItemRequest
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public string Size { get; set; }
        public int Quantity { get; set; }
        public List<string> Paths { get; set; }
    }
}