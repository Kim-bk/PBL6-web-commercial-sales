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
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }
        [Required]
        [MinLength(6)]
        public string PassWord { get; set; }
        [Required]
        [MinLength(6)]
        public string ConfirmPassWord { get; set; }
    }
}
