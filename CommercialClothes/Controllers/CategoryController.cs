using System.Threading.Tasks;
using CommercialClothes.Models.DTOs.Requests;
using CommercialClothes.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CommercialClothes.Commons.CustomAttribute;
using System;
using System.Security.Claims;

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
            if(res.IsSuccess){
                return Ok(res);
            }
            return Ok(res.ErrorMessage);
        }

        [HttpGet("{idCategory:int}")]
        public async Task<IActionResult> GetCategory(int idCategory)
        {
            var res = await _categoryService.GetCategoryAndItemByParentId(idCategory);
            return Ok(res);
        }

        [Authorize]
        [Permission("MANAGE_CHILD_CATEGORY")]
        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] CategoryRequest request)
        {
            int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var rs = await _categoryService.AddCategory(request,userId);
            if (rs.IsSuccess)
            {
                return Ok("Register Category success!");
            }
            return BadRequest(rs.ErrorMessage);
        }

        [Authorize]
        [Permission("MANAGE_PARENT_CATEGORY")]
        [HttpPost("parent")]
        public async Task<IActionResult> AddParentCategory([FromBody] CategoryRequest request)
        {
            int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var rs = await _categoryService.AddParentCategory(request,userId);
            if (rs.IsSuccess)
            {
                return Ok("Register Parent Category success!");
            }
            return BadRequest(rs.ErrorMessage);
        }

        [Authorize]
        [HttpDelete("{idCategory:int}")]
        public async Task<IActionResult> DeleteCategory(int idCategory)
        {
            var rs = await _categoryService.RemoveParentCategory(idCategory);
            if (rs.IsSuccess)
            {
                return Ok("Delete success!");
            }
            return BadRequest(rs.ErrorMessage);
        }

        [Authorize]
        [Permission("MANAGE_PARENT_CATEGORY")]
        [HttpPut]
        public async Task<IActionResult> UpdateCategory([FromBody] CategoryRequest request)
        {
            int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var rs = await _categoryService.UpdateCategoryByCategoryId(request,userId);
            if (rs.IsSuccess)
            {
                return Ok("Update Category success!");
            }
            return BadRequest(rs.ErrorMessage);
        }
    }
}