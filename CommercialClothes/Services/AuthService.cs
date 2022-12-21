using CommercialClothes.Models;
using CommercialClothes.Models.DAL.Interfaces;
using CommercialClothes.Models.DAL;
using CommercialClothes.Services.Base;
using CommercialClothes.Services.Interfaces;
using System.Threading.Tasks;
using CommercialClothes.Services.TokenGenerators;
using System.IdentityModel.Tokens.Jwt;
using System;
using CommercialClothes.Models.DAL.Repositories;
using CommercialClothes.Models.DTOs.Responses;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CommercialClothes.Services
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly AccessTokenGenerator _accessTokenGenerator;
        private readonly RefreshTokenGenerator _refreshTokenGenerator;
        private readonly IRefreshTokenRepository _refreshTokenRepo;
        private readonly IUserRepository _userRepo;

        public AuthService(AccessTokenGenerator accessTokenGenerator, RefreshTokenGenerator refreshTokenGenerator
                , IRefreshTokenRepository refreshTokenRepo, IUnitOfWork unitOfWork
                , IMapperCustom mapper, IUserRepository userRepo) : base(unitOfWork, mapper)
        {
            _accessTokenGenerator = accessTokenGenerator;
            _refreshTokenGenerator = refreshTokenGenerator;
            _refreshTokenRepo = refreshTokenRepo;
            _userRepo = userRepo;
        }

        public async Task<TokenResponse> Authenticate(Account user, string listCredentials, string userGroup = "")
        {
            try
            {
                // Pre-handle user shop ID null
                string userShopId = user.ShopId == null ? "-1" : user.ShopId.ToString();

                // 1. Generate access vs refresh token
                var accessToken = _accessTokenGenerator.Generate(user, userShopId, listCredentials);
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

                await _refreshTokenRepo.AddAsync(userRefreshToken);
                await _unitOfWork.CommitTransaction();

                // 5. Return two tokens (AccessToken vs RefreshToken vs ShopId vs Wallet)
                int shopId = Convert.ToInt32(userShopId);

                // 6. Return wallet according to shop account and custome account
                if (userGroup == "Shop")
                {
                    return new TokenResponse()
                    {
                        IsSuccess = true,
                        AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
                        RefreshToken = refreshTokenHandler,
                        ShopId = Convert.ToInt32(userShopId),
                        Wallet = user.Shop.ShopWallet.HasValue == false ? 0 : user.Shop.ShopWallet.Value
                    };
                }

                return new TokenResponse()
                {
                    IsSuccess = true,
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
                    RefreshToken = refreshTokenHandler,
                    ShopId = Convert.ToInt32(userShopId),
                    Wallet = user.Wallet.HasValue == false ? 0 : user.Wallet.Value
                };
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackTransaction();

                return new TokenResponse()
                {
                    IsSuccess = false,
                    ErrorMessage = e.Message,
                };
            }
        }
    }
}