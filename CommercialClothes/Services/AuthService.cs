using CommercialClothes.Models;
using CommercialClothes.Models.DAL.Interfaces;
using CommercialClothes.Models.DAL;
using CommercialClothes.Services.Base;
using CommercialClothes.Services.Interfaces;
using System.Threading.Tasks;
using CommercialClothes.Services.TokenGenerators;
using System.IdentityModel.Tokens.Jwt;
using System;
using CommercialClothes.Models.DTOs;
using CommercialClothes.Models.DAL.Repositories;

namespace CommercialClothes.Services
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly AccessTokenGenerator _accessTokenGenerator;
        private readonly RefreshTokenGenerator _refreshTokenGenerator;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUserRepository _userRepository;
        public AuthService(AccessTokenGenerator accessTokenGenerator, RefreshTokenGenerator refreshTokenGenerator
                , IRefreshTokenRepository refreshTokenRepository, IUnitOfWork unitOfWork
                , IMapperCustom mapper, IUserRepository userRepository) : base(unitOfWork, mapper)
        {
            _accessTokenGenerator = accessTokenGenerator;
            _refreshTokenGenerator = refreshTokenGenerator;
            _refreshTokenRepository = refreshTokenRepository;
            _userRepository = userRepository;
        }
        public async Task<TokenResponse> Authenticate(Account user)
        {
            try
            {
                // 1. Generate access vs refresh token
                var accessToken = _accessTokenGenerator.Generate(user);
                var refreshToken = _refreshTokenGenerator.Generate();

                // 2. Init refresh token properties
                string refreshTokenId = Guid.NewGuid().ToString();
                string refreshTokenHandler = new JwtSecurityTokenHandler().WriteToken(refreshToken);

                // 3. Create user refresh token
                RefreshToken userRefreshToken = new()
                {
                    Id = refreshTokenId,
                    UserId = user.Id,
                    Token = refreshTokenHandler,
                };

                await _refreshTokenRepository.AddAsync(userRefreshToken);
                await _unitOfWork.CommitTransaction();

                // 5. Return two tokens (AccessToken vs RefreshToken)
                return new TokenResponse()
                {
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
                    RefreshToken = refreshTokenHandler
                };
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackTransaction();
                return new TokenResponse()
                {
                    AccessToken = e.Message,
                };
            }
        }
    }
}
