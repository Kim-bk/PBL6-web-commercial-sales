using System.Security.Claims;
using System;
using System.Threading.Tasks;
using CommercialClothes.Models.DTOs.Requests;
using CommercialClothes.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CommercialClothes.Controllers
{
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
        public async Task<IActionResult> AddCart([FromBody] CartRequest request)
        {
            int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if (await _cartService.AddCart(request,userId))
            {
                return Ok("AddCart success!");
            }
            return BadRequest("Some properties is not valid!");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var res = await _cartService.GetCartById(userId);
            return Ok(res);
        }
    }
}