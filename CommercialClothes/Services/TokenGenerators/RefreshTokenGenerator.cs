using CommercialClothes.Models.DAL.Interfaces;
using CommercialClothes.Models.DAL;
using CommercialClothes.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Configuration;

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
        public async Task<RefreshToken> GetByToken(string token)
        {
            var refreshToken = await _refreshTokenRepository.FindAsync(tk => tk.Token == token);
            if (refreshToken == null)
            {
                throw new ArgumentNullException("Invalid refresh token.");
            }
            return refreshToken;
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
