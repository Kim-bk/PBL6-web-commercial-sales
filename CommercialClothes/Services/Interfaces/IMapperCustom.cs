using System.Collections.Generic;
using CommercialClothes.Models;
using CommercialClothes.Models.DTOs;
using Model.DTOs;

namespace CommercialClothes.Services.Interfaces
{
    public interface IMapperCustom
    {
        List<ItemDTO> MapItems(List<Item> items);
        List<ImageDTO> MapImages(List<Image> images);
        List<CategoryDTO> MapCategories(List<Category> categories);
        List<UserDTO> MapUsers(List<Account> users);
        List<OrderDetailDTO> MapOrderDetails(List<OrderDetail> orderDetails);
        List<OrderDTO> MapOrders(List<Order> orders);
        List<UserGroupDTO> MapUserGroups(List<UserGroup> orders);
    }
}

