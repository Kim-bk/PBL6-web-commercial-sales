using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommercialClothes.Models.DTOs;
using CommercialClothes.Models.DTOs.Requests;
using Microsoft.AspNetCore.Mvc;

namespace CommercialClothes.Services.Interfaces
{
    public interface IItemService
    {
        Task<List<ItemDTO>> GetAllItem();
        Task<List<ItemDTO>> GetItemById(int idItem);
        Task<bool> AddItem(ItemRequest req,int accountId);
        Task<bool> AddItemAvailable(MoreItemRequest req, int ShopId);
        Task<bool> RemoveItemByItemId(int idItem);
        Task<bool> UpdateItemByItemId(ItemRequest req,int accountId);
    }
}