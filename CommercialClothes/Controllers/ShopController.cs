using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommercialClothes.Models.DTOs.Requests;
using CommercialClothes.Services;
using Microsoft.AspNetCore.Mvc;
//using CommercialClothes.Models;

namespace CommercialClothes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopController : Controller
    {
        private readonly IShopService _shopService;
        public ShopController(IShopService shopService)
        {
            _shopService = shopService;
        }
        [HttpGet("{idShop:int}/item")]
        public async Task<IActionResult> GetItem(int idShop)
        {
            var res = await _shopService.GetItemByShopId(idShop);
            return Ok(res);
        }
        [HttpGet("{idShop:int}/category")]
        public async Task<IActionResult> GetCategory(int idShop)
        {
            var res = await _shopService.GetCategories(idShop);
            return Ok(res);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateShop([FromBody] ShopRequest request)
        {
            if (await _shopService.UpdateShop(request))
            {
                return Ok("Update success!");
            }
            else
            {
                return BadRequest("Some properties is not valid!");
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddShop([FromBody] ShopRequest request)
        {
            if (await _shopService.AddShop(request))
            {
                return Ok("Register Shop success!");
            }
            else
            {
                return BadRequest("Some properties is not valid!");
            }
        }
    }
}