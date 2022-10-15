using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ComercialClothes.Models.DTOs.Requests;
using CommercialClothes.Models.DTOs;
using CommercialClothes.Models.DTOs.Requests;
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
                if (!ModelState.IsValid)
                {
                    return BadRequest("Một số thuộc tính không hợp lệ !");
                }

                // 1. Check if refresh token is valid
                var validRefreshToken = _refreshTokenValidator.Validate(refreshRequest.Token);

                if (!validRefreshToken.IsSuccess)
                {
                    return BadRequest(validRefreshToken.ErrorMessage);
                }

                // 2. Get refresh token by token
                var rs = await _refreshTokenGenerator.GetByToken(refreshRequest.Token);

                if (rs.IsSuccess)
                {
                    var refreshTokenDTO = rs.RefreshToken;

                    // 3. Delete that refresh token
                    var deleteRefreshToken = await _refreshTokenGenerator.Delete(refreshTokenDTO.Id);
                    if (!deleteRefreshToken.IsSuccess)
                    {
                        return BadRequest(deleteRefreshToken.ErrorMessage);
                    }

                    // 4. Find user have that refresh token
                    var user = await _userService.FindById(refreshTokenDTO.UserId);

                    // 5. Generate new access token and refresh token to the user
                    TokenResponse response = await _authService.Authenticate(user);

                    return Ok(response);
                }

                return BadRequest(rs.ErrorMessage);
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
    }
}
