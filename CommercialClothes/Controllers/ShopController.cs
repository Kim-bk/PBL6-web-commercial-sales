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

<<<<<<< HEAD
<<<<<<< HEAD
        [Permission("EDIT_SHOP")]
=======
>>>>>>> 7c1ddbc8ab8c9fa71f763c28c534d7d20e53986b
=======
>>>>>>> 7c1ddbc8ab8c9fa71f763c28c534d7d20e53986b
        [HttpPut]
        public async Task<IActionResult> UpdateShop([FromBody] ShopRequest request)
        {
            if (await _shopService.UpdateShop(request))
            {
                return Ok("Update success!");
            }
<<<<<<< HEAD
<<<<<<< HEAD

            return BadRequest("Some properties is not valid!");
        }

        [Permission("EDIT_SHOP")]
=======
            return BadRequest("Some properties is not valid!");
        }

>>>>>>> 7c1ddbc8ab8c9fa71f763c28c534d7d20e53986b
=======
            return BadRequest("Some properties is not valid!");
        }

>>>>>>> 7c1ddbc8ab8c9fa71f763c28c534d7d20e53986b
        [HttpPost]
        public async Task<IActionResult> AddShop([FromBody] ShopRequest request)
        {
            if (await _shopService.AddShop(request))
            {
                return Ok("Register Shop success!");
<<<<<<< HEAD
            }      
            
=======
            }       
<<<<<<< HEAD
>>>>>>> 7c1ddbc8ab8c9fa71f763c28c534d7d20e53986b
=======
>>>>>>> 7c1ddbc8ab8c9fa71f763c28c534d7d20e53986b
            return BadRequest("Some properties is not valid!");
        }
    }
}