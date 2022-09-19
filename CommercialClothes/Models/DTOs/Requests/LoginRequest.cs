using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ComercialClothes.Models.DTOs.Requests
{
    public class LoginRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string PassWord { get; set; }
    }
}
