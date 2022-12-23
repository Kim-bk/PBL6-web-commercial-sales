using CommercialClothes.Models.DTOs.Requests;
using CommercialClothes.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace CommercialClothes.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("vnpay")]
        public async Task<IActionResult> VNPayCheckOut(OrderRequest request)
        {
            try
            {
                var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var result = await _paymentService.VNPayCheckOut(request, userId);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost("stripe")]
        public async Task<IActionResult> StripeCheckOut(OrderRequest request)
        {
            try
            {
                var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var result = await _paymentService.StripeCheckOut(request, userId);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [AllowAnonymous]
        [HttpGet("success")]
        //api/payment/success
        public async Task<IActionResult> PaymentSuccess([FromQuery] string vnp_OrderInfo)
        {
            var rs = await _paymentService.VNPaySuccess(vnp_OrderInfo);
            if (rs)
            {
                return Redirect("https://2clothy.vercel.app/completedpayment");
            }
            else
                return BadRequest("Thanh toán thất bại !");

             
        }
    }
}