using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComercialClothes.Models;
using ComercialClothes.Models.DAL.Repositories;
using ComercialClothes.Models.DTOs.Requests;
using ComercialClothes.Services;
using CommercialClothes.Models.DTOs.Responses;
using CommercialClothes.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CommercialClothes.Controllers
{
    // [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("api/category")]
        public async Task<IActionResult> GetAll()
        {
            var res = await _categoryService.GetAllCategpry();
            return Ok(res);
        }
        [HttpGet("api/item/category/{idCategory:int}")]
        public async Task<IActionResult> GetItem(int idCategory)
        {
            var res = await _categoryService.GetItemByCategoryId(idCategory);
            return Ok(res);
        }
    }
}