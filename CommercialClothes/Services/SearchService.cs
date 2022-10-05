using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommercialClothes.Models.DAL;
using CommercialClothes.Models.DAL.Interfaces;
using CommercialClothes.Models.DAL.Repositories;
using CommercialClothes.Models.DTOs;
using CommercialClothes.Models.DTOs.Responses;
using CommercialClothes.Services.Base;
using CommercialClothes.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CommercialClothes.Services
{
    public class SearchService : BaseService, ISearchService
    {
        private readonly IItemRepository _itemRepository;
        private readonly IUserRepository _userRepository;
        public SearchService(IItemRepository itemRepository, IMapperCustom mapper
            , IUnitOfWork unitOfWork, IUserRepository userRepository) : base(unitOfWork, mapper)
        {
            _itemRepository = itemRepository;
            _userRepository = userRepository;
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
 