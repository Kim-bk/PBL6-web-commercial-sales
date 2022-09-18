using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CommercialClothes.Models.DTOs.Responses
{
    public class ItemResponse
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
    }
}