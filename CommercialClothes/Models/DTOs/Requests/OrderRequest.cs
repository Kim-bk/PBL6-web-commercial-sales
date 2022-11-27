using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ComercialClothes.Models; 

namespace CommercialClothes.Models.DTOs.Requests
{
    public class OrderRequest
    {
       
        [Required]
        public int PaymentId { get; set; }
        public int Total { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public List<CartRequest> Details {get; set;}
    }
}