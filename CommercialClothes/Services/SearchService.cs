using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommercialClothes.Models.DAL;
using CommercialClothes.Models.DAL.Interfaces;
using CommercialClothes.Models.DAL.Repositories;
using CommercialClothes.Models.DTOs;
using CommercialClothes.Services.Base;
using CommercialClothes.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CommercialClothes.Services
{
    public class SearchService : BaseService, ISearchService
    {
        private readonly IItemRepository _itemRepo;
        private readonly IShopRepository _shopRepo;

        public SearchService(IItemRepository itemRepository, IMapperCustom mapper
            , IUnitOfWork unitOfWork, IShopRepository shopRepository) : base(unitOfWork, mapper)
        {
            _itemRepo = itemRepository;
            _shopRepo = shopRepository;
        }

        public async Task<List<ItemDTO>> SearchItem(string keyword)
        {
            if (String.IsNullOrEmpty(keyword))
                return null;

            // 1. Find all items by keyword
            var items = await _itemRepo.SearchItem(keyword);

            // 2. Find all shops relate to keyword
            var shops = await _shopRepo.SearchShopByName(keyword);

            // 3. Map List<Item> to List<ItemDTO>
            var itemsDTO = _mapper.MapItems(items);

            foreach (var shop in shops)
            {
                var i = _mapper.MapItems(shop.Items.ToList());
                itemsDTO.AddRange(i);
            }

            return itemsDTO;
        }
    }
}