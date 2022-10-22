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
        [HttpGet("{idCategory:int}/item")]
        public async Task<IActionResult> GetItemsInCategory(int idCategory)
        {
            var res = await _categoryService.GetCategory(idCategory);
            return Ok(res);
        }
        [HttpGet("{idCategory:int}")]
        public async Task<IActionResult> GetCategory(int idCategory)
        {
            var res = await _categoryService.GetCategoryByParentId(idCategory);
            return Ok(res);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] CategoryRequest request)
        {
            if (await _categoryService.AddCategory(request))
            {
                return Ok("Register Category success!");
            }
            else
            {
                return BadRequest("Some properties is not valid!");
            }
        }

        [Authorize]
        [HttpPost("parent")]
        public async Task<IActionResult> AddParentCategory([FromBody] CategoryRequest request)
        {
            if (await _categoryService.AddParentCategory(request))
            {
                return Ok("Register Parent Category success!");
            }
            else
            {
                return BadRequest("Some properties is not valid!");
            }
        }

        [Authorize]
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

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateCategory([FromBody] CategoryRequest request)
        {
            if (await _categoryService.UpdateCategoryByCategoryId(request))
            {
                return Ok("Update Category success!");
            }
            else
            {
                return BadRequest("Some properties is not valid!");
            }
        }
    }
}