using System;
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
            if (await _orderService.AddOrder(request, userId) != "")
            {
                return Ok("Đặt hàng thành công");
            }

            return BadRequest("Một vài thuộc tính đã bị bỏ sót!");
        }

        [Authorize]
        //[Permission("MANAGE_ORDER")]
        [HttpPut("{idOrder:int}")]
        public async Task<IActionResult> UpdateStatus([FromBody] StatusRequest req, int idOrder)
        {
            var res = await _orderService.UpdateStatusOrder(req, idOrder);
            if (res.IsSuccess == true)
            {
                return Ok("Đã cập nhật trang thái đơn hàng");
            }
            return BadRequest(res.ErrorMessage);
        }

        [Authorize]
        [HttpPut("status/{idOrder:int}")]
        public async Task<IActionResult> SetStatus(int idOrder)
        {
            if (await _orderService.CancelOrder(idOrder))
            {
                return Ok("Đã hủy đơn hàng!");
            }
            return BadRequest("Không tìm thấy đơn hàng!");
        }

        [Authorize]
        [HttpGet("order-details/{orderId:int}")]
        public async Task<IActionResult> GetOrderDetails(int orderId)
        {
            var rs = await _orderService.GetOrderDetails(orderId);
            if (rs.IsSuccess)
            {
                return Ok(rs);
            }
            return BadRequest(rs.ErrorMessage);
        }
    }
}