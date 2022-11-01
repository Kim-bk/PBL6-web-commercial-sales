using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommercialClothes.Models;
using CommercialClothes.Models.DTOs;
using CommercialClothes.Models.DTOs.Requests;

namespace CommercialClothes.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryDTO>> GetAllCategpry();
        List<ItemDTO> GetItemByCategory(List<Item> items);
        Task<CategoryDTO> GetCategory(int idCategory);
        List<ImageDTO> GetImages(List<Image> images);
        Task<bool> RemoveParentCategory(int idCategory);
        Task<List<CategoryDTO>> GetCategoryByParentId(int idCategory);
        Task<bool> AddCategory(CategoryRequest req);
        Task<bool> AddParentCategory(CategoryRequest req);
        Task<bool> UpdateCategory(CategoryRequest req);
        Task<List<CategoryDTO>> GetItem(int parentId);
    }
}