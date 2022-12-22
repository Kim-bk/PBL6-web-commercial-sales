using System.Security.Claims;
using System;
using System.Threading.Tasks;
using CommercialClothes.Models.DTOs.Requests;
using CommercialClothes.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CommercialClothes.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }
        
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> AddCart([FromBody] List<CartRequest> request)
        {
            int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var res = await _cartService.AddCart(request,userId);
            if (res.IsSuccess)
            {
                return Ok("Thêm giỏ hàng thành công!");
            }
            
            return BadRequest(res.ErrorMessage);
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var res = await _cartService.GetCartById(userId);
            return Ok(res);
        }
    }
}