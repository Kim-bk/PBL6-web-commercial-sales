using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ComercialClothes.Models.DTOs.Requests
{
    public class RegistRequest
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [MinLength(6)]
        public string PassWord { get; set; }
        [Required]
        [MinLength(6)]
        public string ConfirmPassWord { get; set; }
    }
}
