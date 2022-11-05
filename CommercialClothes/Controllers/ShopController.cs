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

        [Permission("EDIT_SHOP")]
        [HttpPut]
        public async Task<IActionResult> UpdateShop([FromBody] ShopRequest request)
        {
            if (await _shopService.UpdateShop(request))
            {
                return Ok("Update success!");
            }
            return BadRequest("Shop not found!");
        }
        [HttpPost]
        public async Task<IActionResult> AddShop([FromBody] ShopRequest request)
        {
            if (await _shopService.AddShop(request))
            {
                return Ok("Register Shop success!");    
            }       
            return BadRequest("Name shop is existed!");
        }
    }
}