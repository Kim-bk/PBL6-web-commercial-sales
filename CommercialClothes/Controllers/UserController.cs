using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using ComercialClothes.Models.DTOs.Requests;
using CommercialClothes.Models.DTOs;
using CommercialClothes.Models.DTOs.Requests;
using CommercialClothes.Services;
using CommercialClothes.Services.Interfaces;
using CommercialClothes.Services.TokenGenerators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.DTOs.Responses;

namespace ComercialClothes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly RefreshTokenGenerator _refreshTokenGenerator;
        private readonly IPermissionService _permissionService;
        private readonly IBankService _bankService;

        public UserController(IUserService userService, IAuthService authService
                 , RefreshTokenGenerator refreshTokenGenerator, IPermissionService permissionService
                 , IBankService bankService)
        {
            _userService = userService;
            _authService = authService;
            _refreshTokenGenerator = refreshTokenGenerator;
            _permissionService = permissionService;
            _bankService = bankService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var rs = await _userService.FindById(userId);
            if (rs.IsSuccess)
            {
                return Ok(rs.UserDTO);
            }

            return BadRequest(rs.ErrorMessage);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var rs = await _userService.Login(request);
                if (rs.IsSuccess)
                {
                    // 1. Get list credentials of user
                    var listCredentials = await _permissionService.GetCredentials(rs.User.Id);

                    // 2. Authenticate user
                    var res = await _authService.Authenticate(rs.User, listCredentials);
                    if (res.IsSuccess)
                        return Ok(res);
                    else
                        return BadRequest(res.ErrorMessage);
                }
                return BadRequest(rs.ErrorMessage);
            }
            catch
            {
                throw;
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            _ = await _userService.Logout(userId);
            return Ok("Đăng xuất thành công !");
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest refreshRequest)
        {
            try
            {
                var rs = await _refreshTokenGenerator.Refresh(refreshRequest.Token);
                if (rs.IsSuccess)
                {
                    // 1. Get list credentials of user
                    var listCredentials = await _permissionService.GetCredentials(rs.User.Id);

                    // 2. Authenticate user
                    var responseTokens = await _authService.Authenticate(rs.User, listCredentials);
                    return Ok(responseTokens);
                }

                return BadRequest(rs.ErrorMessage);
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistRequest request)
        {
            var rs = await _userService.Register(request);

            if (rs.IsSuccess)
            {
                return Ok("Vui lòng vào Email kiểm tra tin nhắn !");
            }

            return BadRequest(rs.ErrorMessage);
        }

        [HttpGet("verify-account")]
        public async Task<IActionResult> VerifyAccount([FromQuery] string code)
        {
            var rs = await _userService.CheckUserByActivationCode(new Guid(code));
            if (rs)
            {
                return Ok("Xác thực thành công !");
            }
            return BadRequest("Xác thực thất bại !");
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request)
        {
            var rs = await _userService.ForgotPassword(request.Email);
            if (rs.IsSuccess)
            {
                return Ok("Kiểm tra Email của bạn để thay đổi mật khẩu !");
            }
            return BadRequest(rs.ErrorMessage);
        }

        #region Reset Password

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            var rs = await _userService.ResetPassword(request);
            if (rs.IsSuccess)
            {
                return Ok("Bạn đã thay đổi mật khẩu thành công !");
            }
            return BadRequest(rs.ErrorMessage);
        }

        [HttpGet("reset-password")]
        public async Task<IActionResult> ResetPassword([FromQuery] string code)
        {
            var rs = await _userService.GetUserByResetCode(new Guid(code));
            if (rs)
            {
                // Redirect sang trang cập nhật mật khẩu, gửi kèm theo code
                return Redirect("https://2clothy.vercel.app/resetpassword?code=" + code);
            }
            return BadRequest("Không tìm thấy tài khoản tương ứng !");
        }

        #endregion Reset Password

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateAccount([FromBody] UserRequest request)
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var rs = await _userService.UpdateUser(request, userId);
            if (rs.IsSuccess)
            {
                return Ok("Cập nhật người dùng thành công!");
            }
            return BadRequest(rs.ErrorMessage);
        }

        #region Bank

        [Authorize]
        [HttpPost("bank")]
        public async Task<IActionResult> AddBank([FromBody] BankRequest request)
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var rs = await _bankService.AddBank(request, userId);
            if (rs.IsSuccess)
            {
                return Ok("Thêm thẻ ngân hàng thành công!");
            }
            return BadRequest(rs.ErrorMessage);
        }

        [Authorize]
        [HttpGet("bank")]
        public async Task<IActionResult> GetBank()
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var rs = await _bankService.GetBanksByUser(userId);
            if (rs.IsSuccess)
            {
                return Ok(rs.UserBanks);
            }

            return BadRequest(rs.ErrorMessage);
        }

        [Authorize]
        [HttpPut("bank")]
        public async Task<IActionResult> UpdateBank([FromBody] BankRequest request)
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var rs = await _bankService.UpdateBank(request, userId);
            if (rs.IsSuccess)
            {
                return Ok("Cập nhật thẻ ngân hàng thành công!");
            }
            return BadRequest(rs.ErrorMessage);
        }

        #endregion Bank

        [Authorize]
        [HttpGet("order")]
        public async Task<IActionResult> GetOrders()
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var rs = await _userService.GetOrders(userId);
            if (rs.IsSuccess)
            {
                return Ok(rs.Orders);
            }
            return BadRequest(rs.ErrorMessage);
        }

        [Authorize]
        [HttpGet("transaction")]
        public async Task<List<TransactionResponse>> GetTransactions()
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            return await _userService.GetTransactions(userId);
        }

        [Authorize]
        [HttpGet("wallet")]
        public async Task<int> GetUserWallet()
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            return await _userService.GetAccountWallet(userId);
        }
    }
}