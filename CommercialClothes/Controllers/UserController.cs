using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ComercialClothes.Models.DTOs.Requests;
using CommercialClothes.Models.DTOs.Requests;
using CommercialClothes.Models.DTOs.Responses;
using CommercialClothes.Services;
using CommercialClothes.Services.Interfaces;
using CommercialClothes.Services.TokenGenerators;
using CommercialClothes.Services.TokenValidators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ComercialClothes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly RefreshTokenValidator _refreshTokenValidator;
        private readonly RefreshTokenGenerator _refreshTokenGenerator;

        public UserController(IUserService userService, IAuthService authService
                , RefreshTokenValidator refreshTokenValidator, RefreshTokenGenerator refreshTokenGenerator)
        {
            _userService = userService;
            _authService = authService;
            _refreshTokenValidator = refreshTokenValidator;
            _refreshTokenGenerator = refreshTokenGenerator;
        }
      
        [HttpPost("login")]
        // api/user/login
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userService.Login(request);
            var rs = await _authService.Authenticate(user);
            return Ok(rs);
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

        [Authorize]
        [HttpPost("refresh-token")]
        // api/account/refresh-token
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest refreshRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Some properties is not valid!");
            }

            // 1. Check if refresh token is valid
            _refreshTokenValidator.Validate(refreshRequest.Token);

            // 2. Get refresh token by token
            var refreshTokenDTO = await _refreshTokenGenerator.GetByToken(refreshRequest.Token);

            // 3. Delete that refresh token
            await _refreshTokenGenerator.Delete(refreshTokenDTO.Id);

            // 4. Find user have that refresh token
            var user = await _userService.FindById(refreshTokenDTO.UserId);

            // 5. Generate new access token and refresh token to the user
            TokenResponse response = await _authService.Authenticate(user);

            return Ok(response);
        }

        [HttpPost("register")]
        // api/user/register
        public async Task<IActionResult> Register([FromBody] RegistRequest request)
        {
            if (await _userService.Register(request))
            {
                return Ok("Vui lòng vào Gmail kiểm tra tin nhắn !");
            }
            else
            {
                return BadRequest("Some properties is not valid!");
            }
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
            else
                return BadRequest("Xác thực thất bại !");
        }
    }
}
