using System;
using System.Security.Claims;
using System.Threading.Tasks;
using CommercialClothes.Commons.CustomAttribute;
using CommercialClothes.Models.DTOs.Requests;
using CommercialClothes.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CommercialClothes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : Controller
    {
        private readonly IItemService _itemService;
        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var res = await _itemService.GetAllItem();
            return Ok(res);
        }

        [HttpGet("{idItem:int}")]
        public async Task<IActionResult> GetItemById(int idItem)
        {
            var res = await _itemService.GetItemById(idItem);
            return Ok(res);
        }

        [Authorize]
        [Permission("MANAGE_ITEM")]
        [HttpPost]
        public async Task<IActionResult> AddItem([FromBody] ItemRequest request)
        {
            int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var res = await _itemService.AddItem(request,userId);
            if (res.IsSuccess)
            {
                return Ok("Thêm sản phẩm thành công!");
            }
            return BadRequest(res.ErrorMessage);
        }
        [Authorize]
        [HttpPost("more")]
        public async Task<IActionResult> AddMoreItem([FromBody] MoreItemRequest request)
        {
            int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var res = await _itemService.AddItemAvailable(request,userId);
            if (res.IsSuccess)
            {
                return Ok("Thêm sản phẩm vào cửa hàng thành công!");
            }
            return BadRequest(res.ErrorMessage);
        }

        [Authorize]
        [Permission("MANAGE_ITEM")]
        [HttpDelete("{idItem:int}")]
        public async Task<IActionResult> DeleteItem(int idItem)
        {
            var res = await _itemService.RemoveItemByItemId(idItem);
            if (res.IsSuccess)
            {
                return Ok("Xóa sản phẩm thành công!");
            }
            return BadRequest(res.ErrorMessage);
        }

        [Authorize]
        [Permission("MANAGE_ITEM")]
        [HttpPut]
        public async Task<IActionResult> UpdateItem([FromBody] ItemRequest request)
        {
            int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var res = await _itemService.UpdateItemByItemId(request,userId);
            if (res.IsSuccess)
            {
                return Ok("Cập nhật sản phẩm thành công!");
            }
            return BadRequest(res.ErrorMessage);
        }
    }
}