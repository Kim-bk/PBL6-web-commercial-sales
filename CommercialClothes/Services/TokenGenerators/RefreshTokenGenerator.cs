using CommercialClothes.Models.DAL.Interfaces;
using CommercialClothes.Models.DAL;
using CommercialClothes.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Configuration;
using CommercialClothes.Models.DTOs.Responses;
using CommercialClothes.Models.DAL.Repositories;
using CommercialClothes.Models.DTOs;
using CommercialClothes.Services.Interfaces;
using CommercialClothes.Services.TokenValidators;

namespace CommercialClothes.Services.TokenGenerators
{
    public class RefreshTokenGenerator
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly TokenGenerator _tokenGenerator;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly RefreshTokenValidator _refreshTokenValidator;
        public RefreshTokenGenerator(IRefreshTokenRepository refreshTokenRepository, TokenGenerator tokenGenerator
                        , IConfiguration configuration, IUnitOfWork unitOfWork, RefreshTokenValidator refreshTokenValidator
                        , IUserRepository userRepository)
        {
            _unitOfWork = unitOfWork;
            _refreshTokenRepository = refreshTokenRepository;
            _tokenGenerator = tokenGenerator;
            _configuration = configuration;
            _userRepository = userRepository;
            _refreshTokenValidator = refreshTokenValidator;
        }
        public async Task<UserResponse> Refresh(string tokenContent)
        {
            // 1. Check if refresh token is valid
            var validRefreshToken = _refreshTokenValidator.Validate(tokenContent);

            if (!validRefreshToken.IsSuccess)
            {
                return new UserResponse
                {
                    IsSuccess = false,
                    ErrorMesage = validRefreshToken.ErrorMessage
                };
            }

            // 2. Get refresh token by token
            var rs = await GetByToken(tokenContent);

            if (!rs.IsSuccess)
            {
                return new UserResponse
                {
                    IsSuccess = false,
                    ErrorMesage = rs.ErrorMessage
                };
            }

            var refreshTokenDTO = rs.RefreshToken;

            // 3. Delete that refresh token
            var deleteRefreshToken = await Delete(refreshTokenDTO.Id);
            if (!deleteRefreshToken.IsSuccess)
            {
                return new UserResponse
                {
                    IsSuccess = false,
                    ErrorMesage = deleteRefreshToken.ErrorMessage
                };
            }

            // 4. Find user have that refresh token
            return new UserResponse
            {
                IsSuccess = true,
                User = refreshTokenDTO.User
            };
        }

        public JwtSecurityToken Generate()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:RefreshTokenSecret"]));
            var issuer = _configuration["AuthSettings:Issuer"];
            var audience = _configuration["AuthSettings:Audience"];
            var expires = DateTime.UtcNow.AddMonths(4); // expires in 4 months later

            return _tokenGenerator.GenerateToken(key, issuer, audience, expires);
        }
        private async Task<RefreshTokenResponse> GetByToken(string token)
        {
            var refreshToken = await _refreshTokenRepository.FindAsync(tk => tk.Token == token);

            if (refreshToken == null)
            {
                return new RefreshTokenResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "Refresh Token không tìm thấy trong cơ sở dữ liệu !",
                };
            }

            return new RefreshTokenResponse
            {
                IsSuccess = true,
                RefreshToken = refreshToken
            };
        }
        private async Task<RefreshTokenResponse> Delete(string tokenId)
        {
            try
            {
                _refreshTokenRepository.Delete(tk => tk.Id == tokenId);
                await _unitOfWork.CommitTransaction();
                return new RefreshTokenResponse
                {
                    IsSuccess = true,
                };
            }
            catch (Exception e)
            {
                return new RefreshTokenResponse
                {
                    IsSuccess = false,
                    ErrorMessage = e.Message,
                };
            }
        }
    }
}
