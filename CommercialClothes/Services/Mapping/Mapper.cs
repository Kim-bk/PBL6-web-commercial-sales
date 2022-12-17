using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CommercialClothes.Services.Interfaces;
using CommercialClothes.Models;
using CommercialClothes.Models.DTOs;
using Model.DTOs;

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
                    ShopName = i.Shop.Name,
                    Name = i.Name,
                    Price = i.Price,
                    DateCreated = i.DateCreated,
                    Description = i.Description,
                    Size = i.Size,
                    Quantity = i.Quantity,
                    CategoryName = i.Category.Name,
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

        public List<CategoryDTO> MapCategories(List<Category> categories)
        {
            var storeCategories = new List<CategoryDTO>();
            foreach(var i in categories)
            {
                var category = new CategoryDTO 
                {
                    Id = i.Id,
                    Name = i.Name,
                    ParentId = i.ParentId,
                    Description = i.Description,
                    Gender = i.Gender,
                    ShopId = i.ShopId,
                    Items = MapItems((i.Items).OrderByDescending(p => i.Id).ToList()),
                    ImagePath = i.Image.Path,
                 };
                storeCategories.Add(category);
            }
            return storeCategories;            
        }
        public List<UserDTO> MapUsers(List<Account> users)
        {
            return _autoMapper.Map<List<Account>, List<UserDTO>>(users);
        }

        public List<OrderDetailDTO> MapOrderDetails(List<OrderDetail> orderDetails)
        {
            var storeOrderDetails = new List<OrderDetailDTO>();
            foreach (var i in orderDetails)
            {
                var orderDetailDTO = new OrderDetailDTO
                {
                    Id = i.Id,
                    ItemId = i.ItemId,
                    ItemName = i.Item.Name,
                    Size = i.Item.Size,
                    Price = i.Price,
                    Quantity = i.Quantity.Value,
                    ItemImg = i.Item.Images.Select(i => i.Path).First(),

                };
                storeOrderDetails.Add(orderDetailDTO);
            }
            return storeOrderDetails;

            //return _autoMapper.Map<List<OrderDetail>, List<OrderDetailDTO>>(orderDetails);
        }

        public List<OrderDTO> MapOrders(List<Order> orders)
        {
            return _autoMapper.Map<List<Order>, List<OrderDTO>>(orders);
        }

        public List<UserGroupDTO> MapUserGroups(List<UserGroup> userGroups)
        {
            return _autoMapper.Map<List<UserGroup>, List<UserGroupDTO>>(userGroups);
        }
    }
}
