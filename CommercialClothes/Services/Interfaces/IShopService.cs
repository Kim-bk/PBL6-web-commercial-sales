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
        List<ItemDTO> GetItemByShop(List<Item> items);
        Task<List<ShopDTO>> GetItemByShopId(int idCategory);
        List<CategoryDTO> GetCategoriesByShop(List<Category> categories);
        Task<List<ShopDTO>> GetCategories(int idShop);
        Task<bool> UpdateShop(ShopRequest req);
        Task<bool> AddShop(ShopRequest req);
        Task<ShopDTO> GetShop(int idShop);

    }
}