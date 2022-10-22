using System.Collections.Generic;
using CommercialClothes.Models;
using CommercialClothes.Models.DTOs;

namespace CommercialClothes.Services.Interfaces
{
    public interface IMapperCustom
    {
        List<ItemDTO> MapItems(List<Item> items);
        List<ImageDTO> MapImages(List<Image> images);
        List<CategoryDTO> MapCategories(List<Category> categories);
        List<UserDTO> MapUsers(List<Account> users);
        List<CategoryDTO> MapCategoriesGetItem(List<Category> categories);
    }
}

