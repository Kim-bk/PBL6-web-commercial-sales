using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComercialClothes.Models;
using ComercialClothes.Models.DAL.Repositories;
using ComercialClothes.Models.DTOs.Requests;
using ComercialClothes.Services;
using CommercialClothes.Models.DTOs.Responses;
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
        // public List<Item> items = new List<Item>();
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllItem()
        {
            var items = await _itemService.GetAllItem();  
            return new OkObjectResult(new
            {
                items.Name,
                items.Price,
                items.Description,
            });
        }
    }
}