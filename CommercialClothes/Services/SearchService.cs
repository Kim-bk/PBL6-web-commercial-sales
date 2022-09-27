using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComercialClothes.Models.DAL;
using CommercialClothes.Models.DAL.Interfaces;
using CommercialClothes.Models.DTOs;
using CommercialClothes.Services.Base;
using CommercialClothes.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CommercialClothes.Services
{
    public class SearchService : BaseService, ISearchService
    {
        private readonly IItemRepository _itemRepository;
        private readonly IMapperCustom _mapper;
        public SearchService(IItemRepository itemRepository, IMapperCustom mapper
            , IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _itemRepository = itemRepository;
            _mapper = mapper;
        }
        
        public async Task<List<ItemDTO>> SearchItem(string keyword)
        {
            if (String.IsNullOrEmpty(keyword))
                return null;

            // 1. Find all items by keyword
            var items = await _itemRepository.SearchItem(keyword);

            // 2. Map List<Item> to List<ItemDTO>
            return _mapper.MapItems(items);
        }
    }
}
 