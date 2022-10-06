using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommercialClothes.Models;
using CommercialClothes.Models.DAL.Repositories;
using CommercialClothes.Models.DTOs.Requests;
using CommercialClothes.Services;
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
        [HttpPost]
        public async Task<IActionResult> AddItem([FromBody] ItemRequest request)
        {
            if (await _itemService.AddItem(request))
            {
                return Ok("Register success!");
            }
            else
            {
                return BadRequest("Some properties is not valid!");
            }
        }
        [HttpDelete("{idItem:int}")]
        public async Task<IActionResult> DeleteItem(int idItem)
        {
            if (await _itemService.RemoveItemByItemId(idItem))
            {
                return Ok("Delete success!");
            }
            else
            {
                return BadRequest("Some properties is not valid!");
            }
            
        }
        [HttpPut]
        public async Task<IActionResult> UpdateItem([FromBody] ItemRequest request)
        {
            if (await _itemService.UpdateItemByItemId(request))
            {
                return Ok("Update success!");
            }
            else
            {
                return BadRequest("Some properties is not valid!");
            }
        }
    }
}