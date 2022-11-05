using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CommercialClothes.Commons.CustomAttribute;
using CommercialClothes.Models.DTOs.Requests;
using CommercialClothes.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CommercialClothes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] OrderRequest request)
        {
            int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if (await _orderService.AddOrder(request,userId))
            {
                return Ok("AddOrder success!");
            }
            return BadRequest("Input attribute is missing!");
        }

        
        [Authorize]
        [Permission("MANAGE_ORDER")]
        [HttpPut("{idOrder:int}")]
        public async Task<IActionResult> UpdateStatus([FromBody] StatusRequest req,int idOrder)
        {
            var res = await _orderService.UpdateStatusOrder(req,idOrder);
            if (res.IsSuccess == true)
            {
                return Ok("UpdateStatus success!");
            }
            return BadRequest(res.ErrorMessage);
        }

        [Authorize]
        [HttpPut("status/{idOrder:int}")]
        public async Task<IActionResult> SetStatus(int idOrder)
        {
            if (await _orderService.CancelOrder(idOrder))
            {
                return Ok("Cancel order success!");
            }
            return BadRequest("Order not found!");
        }
    }
}