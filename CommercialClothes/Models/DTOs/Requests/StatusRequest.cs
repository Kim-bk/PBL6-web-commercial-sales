using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CommercialClothes.Models.DTOs.Requests.Base;

namespace CommercialClothes.Models.DTOs.Requests
{
    public class StatusRequest
    {
        [Required]
        public int StatusId { get; set; }
    }
}