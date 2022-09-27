using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ComercialClothes.Models;
using CommercialClothes.Models.DTOs;
using CommercialClothes.Services.Interfaces;

namespace CommercialClothes.Services.Mapping
{
    public class Mapper : IMapperCustom
    {
        private readonly IMapper _autoMapper;
        public Mapper(IMapper autoMapper)
        {
            _autoMapper = autoMapper;
        }
        public List<ItemDTO> MapItems(List<Item> items)
        {
            var storeItems = new List<ItemDTO>();
            foreach(var i in items)
            {
                var item = new ItemDTO 
                {
                    Id = i.Id,
                    CategoryId = i.CategoryId, 
                    ShopId = i.ShopId,
                    Name = i.Name,
                    Price = i.Price,
                    DateCreated = i.DateCreated,
                    Description = i.Description,
                    Size = i.Size,
                    Quantity = i.Quantity,
                    Images = MapImages((i.Images).ToList()),
                 };
                storeItems.Add(item);
            }
            return storeItems;
        }

        public List<ImageDTO> MapImages(List<Image> images)
        {
            return _autoMapper.Map<List<Image>, List<ImageDTO>>(images);
        }
    }
}
