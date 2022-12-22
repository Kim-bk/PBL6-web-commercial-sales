using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CommercialClothes.Models.DTOs.Requests
{
    public class CategoryRequest
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool Gender { get; set; }   
        public string? ImagePath { get; set; }   
    }
}