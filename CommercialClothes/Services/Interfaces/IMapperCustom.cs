using System.Collections.Generic;
using CommercialClothes.Models;
using CommercialClothes.Models.DTOs.Responses;

namespace CommercialClothes.Services.Interfaces
{
    public interface IMapperCustom
    {
        List<ItemDTO> MapItems(List<Item> items);
        List<ImageDTO> MapImages(List<Image> images);
    }
}