using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComercialClothes.Models.DTOs.Requests;
using CommercialClothes.Models;
using CommercialClothes.Models.DTOs;
using CommercialClothes.Models.DTOs.Requests;
using CommercialClothes.Models.DTOs.Responses;
using Model.DTOs.Responses;

namespace CommercialClothes.Services
{
    public interface IShopService
    {
        public Task<UserResponse> Login(LoginRequest request);

        public Task<List<ShopDTO>> GetItemByShopId(int idShop);

        public Task<List<ShopDTO>> GetCategories(int idShop);

        public Task<ShopResponse> UpdateShop(ShopRequest req, int accountId);

        public Task<ShopResponse> AddShop(ShopRequest req, int accountId);

        public Task<ShopDTO> GetShop(int idShop);

        public Task<List<OrderDTO>> GetOrder(int idShop);

        public Task<List<TransactionResponse>> GetTransactions(int shopId);
    }
}