using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CommercialClothes.Models.DTOs.Requests
{
    public class ShopRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public int IdUser { get; set; }
        public string Description { get; set; }
        public List<string> Paths { get; set; }
    }
}