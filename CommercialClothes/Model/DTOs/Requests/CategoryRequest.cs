using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CommercialClothes.Models.DTOs.Requests
{
    public class CategoryRequest
    {
        [Required]
        public int Id { get; set; }
        public int ParentId { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public bool Gender { get; set; }   
        public string? ImagePath { get; set; }   
    }
}