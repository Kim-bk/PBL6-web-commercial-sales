using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CommercialClothes.Models;

namespace CommercialClothes.Models.DTOs.Requests
{
    public class ImageRequest
    {
        [Required]
        public int ShopId { get; set; }
        [Required]
        public string Path { get; set; }
        [Required]
        public virtual Item Item { get; set; }

    }
}