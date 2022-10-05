using CommercialClothes.Models.DTOs.Responses;
using System.Collections.Generic;
using ComercialClothes.Models;
using CommercialClothes.Models.DTOs;
using System.Threading.Tasks;

namespace CommercialClothes.Services.Interfaces
{
    public interface IMapperCustom
    {
        List<ItemDTO> MapItems(List<Item> items);
        List<ImageDTO> MapImages(List<Image> images);
        List<CategoryDTO> MapCategories(List<Category> categories);
        List<UserDTO> MapUsers(List<Account> users);
    }
}

