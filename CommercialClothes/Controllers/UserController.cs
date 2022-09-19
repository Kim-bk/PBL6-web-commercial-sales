using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComercialClothes.Models.DTOs.Requests;
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
            if (await _userService.Login(request))
            {
                return Ok("Login success!");
            }
            else
            {
                return BadRequest("Some properties is not valid!");
            }
        }

        [HttpPost("register")]
        // api/user/register
        public async Task<IActionResult> Register([FromBody] RegistRequest request)
        {
            if (await _userService.Register(request))
            {
                return Ok("Register success!");
            }
            else
            {
                return BadRequest("Some properties is not valid!");
            }
        }

        [HttpGet("verify-account")]
        public async Task<IActionResult> VerifyAccount([FromQuery] string activationCode)
        {
            // 1. Check user with the activation code
            var rs = await _userService.CheckUserByActivationCode(new Guid(activationCode));

            if (rs)
            {
                return Ok("Verify account success!");
            }

            else
            {
                return BadRequest("Verify account failed!");
            }

        }
    }
}
