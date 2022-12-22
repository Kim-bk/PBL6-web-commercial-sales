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
        public int ShopId { get; set; }
        public string Path { get; set; }
        public virtual Item Item { get; set; }

    }
}