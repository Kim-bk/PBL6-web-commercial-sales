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

namespace CommercialClothes.Services.TokenGenerators
{
    public class RefreshTokenGenerator
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly TokenGenerator _tokenGenerator;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        public RefreshTokenGenerator(IRefreshTokenRepository refreshTokenRepository, TokenGenerator tokenGenerator,
                                IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _refreshTokenRepository = refreshTokenRepository;
            _tokenGenerator = tokenGenerator;
            _configuration = configuration;
        }

        public JwtSecurityToken Generate()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:RefreshTokenSecret"]));
            var issuer = _configuration["AuthSettings:Issuer"];
            var audience = _configuration["AuthSettings:Audience"];
            var expires = DateTime.UtcNow.AddMonths(4); // expires in 4 months later

            return _tokenGenerator.GenerateToken(key, issuer, audience, expires);
        }
        public async Task<RefreshTokenResponse> GetByToken(string token)
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

        public async Task Delete(string tokenId)
        {
            try
            {
                _refreshTokenRepository.Delete(tk => tk.Id == tokenId);
                await _unitOfWork.CommitTransaction();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
