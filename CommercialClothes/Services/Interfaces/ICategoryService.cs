using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommercialClothes.Models;
using CommercialClothes.Models.DTOs;
using CommercialClothes.Models.DTOs.Requests;
using CommercialClothes.Models.DTOs.Response;

namespace CommercialClothes.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryDTO>> GetAllCategpry();
        Task<CategoryDTO> GetCategory(int idCategory);
        Task<CategoryResponse> RemoveParentCategory(int idCategory);
        Task<List<CategoryDTO>> GetCategoryByParentId(int idCategory);
        Task<CategoryDTO> GetCategoryAndItemByParentId(int idCategory);
        Task<CategoryDTO> AddCategory(CategoryRequest req,int accountId);
        Task<CategoryResponse> AddParentCategory(CategoryRequest req,int accountId);
        Task<CategoryResponse> UpdateCategoryByCategoryId(CategoryRequest req,int accountId);
    }
}