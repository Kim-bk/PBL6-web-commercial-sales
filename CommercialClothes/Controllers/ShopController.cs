using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    }
}