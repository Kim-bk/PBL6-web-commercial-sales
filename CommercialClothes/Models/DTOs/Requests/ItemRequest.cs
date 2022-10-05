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
        [Required]
        public int Id { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int ShopId { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Size { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public List<string> Paths { get; set; }
    }
}