using System.Security.Claims;
using System;
using System.Threading.Tasks;
using CommercialClothes.Models.DTOs.Requests;
using CommercialClothes.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CommercialClothes.Controllersce
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

        [HttpPut]
        public async Task<IActionResult> AddItem([FromBody] CartRequest request)
        {
            if (await _cartService.AddCart(request))
            {
                return Ok("AddCart success!");
            }
            else
            {
                return BadRequest("Some properties is not valid!");
            }
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