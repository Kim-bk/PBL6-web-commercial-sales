using System;
using System.Security.Claims;
using System.Threading.Tasks;
using CommercialClothes.Commons.CustomAttribute;
using CommercialClothes.Models.DTOs.Requests;
using CommercialClothes.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CommercialClothes.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]

    public class ShopController : Controller
    {
        private readonly IShopService _shopService;
        public ShopController(IShopService shopService)
        {
            _shopService = shopService;
        }

        [AllowAnonymous]
        [HttpGet("{idShop:int}")]
        public async Task<IActionResult> ViewShop(int idShop)
        {
            var res = await _shopService.GetShop(idShop);
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> ViewShopAuthorize()
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var res = await _shopService.GetShopAuthorize(userId);
            return Ok(res);
        }

        [AllowAnonymous]
        [HttpGet("{idShop:int}/item")]
        public async Task<IActionResult> GetItem(int idShop)
        {
            var res = await _shopService.GetItemByShopId(idShop);
            return Ok(res);
        }

        [AllowAnonymous]
        [HttpGet("{idShop:int}/category")]
        public async Task<IActionResult> GetCategory(int idShop)
        {
            try
            {
                var res = await _shopService.GetCategories(idShop);
                return Ok(res);
            }    
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
           
        }

        [Permission("EDIT_SHOP")]
        [HttpPut]
        public async Task<IActionResult> UpdateShop([FromBody] ShopRequest request)
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var res = await _shopService.UpdateShop(request,userId);
            if (res.IsSuccess)
            {
                return Ok("Cập nhật cửa hàng thành công!");
            }
            return BadRequest(res.ErrorMessage);
        }

        // [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AddShop([FromBody] ShopRequest request)
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var rs = await _shopService.AddShop(request,userId);
            if (rs.IsSuccess == true)
            {
                return Ok("Đăng ký cửa hàng thành công!");    
            }       
            return BadRequest(rs.ErrorMessage);
        }
        [HttpGet("order")]
        public async Task<IActionResult> GetOrder()
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var res = await _shopService.GetOrder(userId);
            return Ok(res);
        }
    }
}