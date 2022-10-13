using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommercialClothes.Models;
using CommercialClothes.Models.DTOs;

namespace CommercialClothes.Services
{
    public interface IShopService
    {
        List<ItemDTO> GetItemByShop(List<Item> items);
        Task<List<ShopDTO>> GetItemByShopId(int idCategory);
    }
}