using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComercialClothes.Models.DTOs.Requests;
using ComercialClothes.Services;
using Microsoft.AspNetCore.Mvc;

namespace CommercialClothes.Controllers
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
    }
}
