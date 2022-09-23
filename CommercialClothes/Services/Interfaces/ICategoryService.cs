using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComercialClothes.Models;
using CommercialClothes.Models.DTOs.Responses;

namespace CommercialClothes.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryDTO>> GetAllCategpry();
        List<ItemDTO> GetItemByCategory(List<Item> items);
        Task<List<CategoryDTO>> GetItemByCategoryId(int idCategory);
        List<ImageDTO> GetImages(List<Image> images);
    }
}