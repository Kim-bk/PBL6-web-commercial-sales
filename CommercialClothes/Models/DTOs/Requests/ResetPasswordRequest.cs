using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CommercialClothes.Models.DTOs.Requests
{
    public class ResetPasswordRequest
    {
        [Required]
        public Guid ResetPasswordCode { get; set; }

        [RegularExpression("/(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9]).{8}/g",
      ErrorMessage = "Password must meet requirements")]
        public string NewPassword { get; set; }

        [RegularExpression("/(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9]).{8}/g",
       ErrorMessage = "Password must meet requirements")]
        [Compare("NewPassword", ErrorMessage = "Confirm password not match !")]
        public string ConfirmPassword { get; set; }
    }
}
