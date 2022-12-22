using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CommercialClothes.Models.DTOs.Requests
{
    public class StatisticalRequest
    {
        public int ShopId { get; set; }
        public string From { get; set; }
        public string To { get; set; }  
    }
}