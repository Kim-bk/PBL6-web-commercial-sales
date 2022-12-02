using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommercialClothes.Models;
using CommercialClothes.Models.DTOs;
using CommercialClothes.Models.DTOs.Requests;
using CommercialClothes.Models.DTOs.Responses;

namespace CommercialClothes.Services
{
    public interface IShopService
    {
        Task<List<ShopDTO>> GetItemByShopId(int idShop);
        Task<List<ShopDTO>> GetCategories(int idShop);
        Task<ShopResponse> UpdateShop(ShopRequest req,int accountId);
        Task<ShopResponse> AddShop(ShopRequest req,int accountId);
        Task<ShopDTO> GetShop(int idShop);
        Task<ShopDTO> GetShopAuthorize(int idShop);
        Task<List<OrderDTO>> GetOrder(int idShop);
    }
}