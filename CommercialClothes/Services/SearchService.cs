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
        private readonly IItemRepository _itemRepository;
        private readonly IShopRepository _shopRepository;
        public SearchService(IItemRepository itemRepository, IMapperCustom mapper
            , IUnitOfWork unitOfWork, IShopRepository shopRepository) : base(unitOfWork, mapper)
        {
            _itemRepository = itemRepository;
            _shopRepository = shopRepository;
        }

        public async Task<List<ItemDTO>> SearchItem(string keyword)
        {
            if (String.IsNullOrEmpty(keyword))
                return null;

            // 1. Find all items by keyword
            var items = await _itemRepository.SearchItem(keyword);

            // 2. Find all shops relate to keyword
            var shops = await _shopRepository.SearchShopByName(keyword);

            // 3. Map List<Item> to List<ItemDTO>
            var itemsDTO = _mapper.MapItems(items);
             
            foreach(var shop in shops)
            {
                var i = _mapper.MapItems(shop.Items.ToList());
                itemsDTO.AddRange(i);
            }

            return itemsDTO;
        }
    }
}
 