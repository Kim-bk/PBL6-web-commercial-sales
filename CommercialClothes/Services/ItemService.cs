using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComercialClothes.Models;
using ComercialClothes.Models.DAL;
using ComercialClothes.Models.DAL.Repositories;
using CommercialClothes.Models.DTOs.Responses;
using CommercialClothes.Services.Base;
using CommercialClothes.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CommercialClothes.Services
{
    public class ItemService : BaseService, IItemService
    {
        private readonly IItemRepository _itemRepository;

        public ItemService(IItemRepository itemRepository ,IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _itemRepository = itemRepository;
        }

        public async Task<ItemResponse> GetAllItem()
        {
            var items = await _itemRepository.GetAllItem();
            return items;
            
        }
    }
}