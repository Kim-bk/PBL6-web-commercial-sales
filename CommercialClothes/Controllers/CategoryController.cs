using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommercialClothes.Models;
using CommercialClothes.Models.DAL.Repositories;
using CommercialClothes.Models.DTOs.Requests;
using CommercialClothes.Services;
using CommercialClothes.Models.DTOs.Responses;
using CommercialClothes.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CommercialClothes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var res = await _categoryService.GetAllCategpry();
            return Ok(res);
        }
        [HttpGet("{idCategory:int}")]
        public async Task<IActionResult> GetItem(int idCategory)
        {
            var res = await _categoryService.GetItemByCategoryId(idCategory);
            return Ok(res);
        }
        [HttpDelete("{idCategory:int}")]
        public async Task<IActionResult> DeleteItem(int idCategory)
        {
            if (await _categoryService.RemoveParentCategory(idCategory))
            {
                return Ok("Delete success!");
            }
            else
            {
                return BadRequest("Some properties is not valid!");
            }
            
        }
    }
}