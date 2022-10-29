using System;
using System.Security.Claims;
using System.Threading.Tasks;
using ComercialClothes.Models.DTOs.Requests;
using CommercialClothes.Models.DTOs.Requests;
using CommercialClothes.Services;
using CommercialClothes.Services.Interfaces;
using CommercialClothes.Services.TokenGenerators;
using CommercialClothes.Services.TokenValidators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ComercialClothes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly RefreshTokenGenerator _refreshTokenGenerator;

        public UserController(IUserService userService, IAuthService authService
                 , RefreshTokenGenerator refreshTokenGenerator)
        {
            _userService = userService;
            _authService = authService;
            _refreshTokenGenerator = refreshTokenGenerator;
        }
      
        [HttpPost("login")]
        // api/user/login
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var rs = await _userService.Login(request);
            if (rs.IsSuccess)
            {
                var res = await _authService.Authenticate(rs.User);
                return Ok(res);
            }    
            
            return BadRequest(rs.ErrorMesage);
        }

        [Authorize]
        [HttpPost("logout")]
        // api/account/logout
        public async Task<IActionResult> Logout()
        {
            int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            _ = await _userService.Logout(userId);
            return Ok("Đăng xuất thành công !");
        }

        [HttpPost("refresh-token")]
        // api/account/refresh-token
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest refreshRequest)
        {
            try
            {
                var rs = await _refreshTokenGenerator.Refresh(refreshRequest.Token);
                if (rs.IsSuccess)
                {
                    var responseTokens = await _authService.Authenticate(rs.User);
                    return Ok(responseTokens);
                }

                return BadRequest(rs.ErrorMesage);
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
           
        }

        [HttpPost("register")]
        // api/user/register
        public async Task<IActionResult> Register([FromBody] RegistRequest request)
        {
            var rs = await _userService.Register(request);

            if (rs.IsSuccess)
            {
                return Ok("Vui lòng vào Email kiểm tra tin nhắn !");
            }

            return BadRequest(rs.ErrorMesage);
        }

        [HttpGet("verify-account")]
        // api/user/verify-account?code
        public async Task<IActionResult> VerifyAccount([FromQuery] string code)
        {
            var rs = await _userService.CheckUserByActivationCode(new Guid(code));
            if (rs)
            {
                return Ok("Xác thực thành công !");
            }

            return BadRequest("Xác thực thất bại !");
        }
        [HttpPut]
        // api/user/
        public async Task<IActionResult> UpdateAccount([FromBody] UserRequest request)
        {
            var rs = await _userService.UpdateUser(request);
            if (rs.IsSuccess)
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
