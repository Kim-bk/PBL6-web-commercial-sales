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

        public async Task<List<ItemDTO>> GetAllItem()
        {
            var listItems = await _itemRepository.GetAll();

            var listItemsDTO = new List<ItemDTO>();

            foreach(var item in listItems)
            {
                var itemDTO = new ItemDTO()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Price = item.Price,
                    Description = item.Description,
                    // ShopId = item.ShopId,
                    // Images = item.GetImages(Images),
                    Images = GetImages(item.Images.ToList()),
                    
                };
                listItemsDTO.Add(itemDTO);
            }
            return listItemsDTO;
        }
        public List<ImageDTO> GetImages(List<Image> images)
        {
            var listImageDTO = new List<ImageDTO>();
            foreach (var item in images)
            {
                var imageDTO = new ImageDTO()
                {
                    ShopId = item.ShopId,
                    Path = item.Path,
                };
                listImageDTO.Add(imageDTO);
            }
            return listImageDTO;
        }
        // viet 1 ham map Images rieng o ngoai nay (trueyn vao la List<Image>)
        public async Task<List<ItemIdDTO>> GetItembyID(int id)
        {
            var item = await _itemRepository.FindAsync(p => p.Id == id);
            var itembyId = new List<ItemIdDTO>();
            if (item == null)
            {
                throw new Exception("Item not found!!!!!!!");
            }
            var items = new ItemIdDTO()
            {
                Id = item.Id,
                CategoryId = item.CategoryId,
                Name = item.Name,
                Price = item.Price,
                Description = item.Description,
                DateCreated = item.DateCreated,
                Quantity = item.Quantity,
                Size = item.Size,
                Images = GetImages(item.Images.ToList()),
            };
            itembyId.Add(items);
            return itembyId;
                
        }
    }
}