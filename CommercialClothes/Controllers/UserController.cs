using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComercialClothes.Models.DTOs.Requests;
using CommercialClothes.Models.DTOs.Requests;
using CommercialClothes.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComercialClothes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
      
        [HttpPost("login")]
        // api/user/login
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            await _userService.Login(request);
            return Ok("Login success!");
        }

        [HttpPost("register")]
        // api/user/register
        public async Task<IActionResult> Register([FromBody] RegistRequest request)
        {
            await _userService.Register(request);
            return Ok("Register success!");
        }

        [HttpGet("verify-account")]
        public async Task<IActionResult> VerifyAccount([FromQuery] string code)
        {
            // 1. Check user with the activation code
            var rs = await _userService.CheckUserByActivationCode(new Guid(code));

            if (rs)
            {
                return Ok("Verify account success!");
            }
            return BadRequest("Verify account failed!");
        }

        [HttpGet("reset-password")]
        public async Task<IActionResult> ResetPassword([FromQuery] string code)
        {
            if (await _userService.GetUserByResetCode(new Guid(code)))
            {
                ResetPasswordRequest model = new ResetPasswordRequest();
                model.ResetPasswordCode = new Guid(code);
                // return api reset password
                return View(model);
            }
            else return BadRequest();
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            await _userService.ResetPassword(request);
            return Ok("Reset password success !");
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] string userEmail)
        {
            await _userService.ForgotPassword(userEmail);
            return Ok("Email has been sent to your email !");
        }
    }
}
