using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommercialClothes.Models.DTOs;
using CommercialClothes.Models.DTOs.Requests;
using CommercialClothes.Models.DTOs.Responses;
using Microsoft.AspNetCore.Mvc;

namespace CommercialClothes.Services.Interfaces
{
    public interface IItemService
    {
        Task<List<ItemDTO>> GetAllItem();
        Task<ItemDTO> GetItemById(int idItem);
        Task<ItemResponse> AddItem(ItemRequest req,int accountId);
        Task<ItemResponse> AddItemAvailable(MoreItemRequest req, int ShopId);
        Task<ItemResponse> RemoveItemByItemId(int idItem);
        Task<ItemResponse> UpdateItemByItemId(ItemRequest req,int accountId);
    }
}