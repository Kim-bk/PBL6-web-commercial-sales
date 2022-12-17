using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CommercialClothes.Models.DTOs.Requests
{
    public class StatisticalRequest
    {
        [Required]
        public int ShopId { get; set; }
        [Required]
        public string From { get; set; }
        [Required]
        public string To { get; set; }  
    }
}