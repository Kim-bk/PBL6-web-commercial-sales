using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommercialClothes.Models;
using CommercialClothes.Models.DAL;
using CommercialClothes.Models.DAL.Repositories;
using CommercialClothes.Models.DTOs;
using CommercialClothes.Models.DTOs.Requests;
using CommercialClothes.Models.DTOs.Response;
using CommercialClothes.Services.Base;
using CommercialClothes.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CommercialClothes.Services
{
    public class CategoryService : BaseService, ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IImageRepository _imageRepository;
        public CategoryService(ICategoryRepository categoryRepository ,IUnitOfWork unitOfWork,IMapperCustom mapper, IImageRepository imageRepository) : base(unitOfWork, mapper)
        {
            _categoryRepository = categoryRepository;
            _imageRepository = imageRepository;
        }

        public async Task<CategoryDTO> AddCategory(CategoryRequest req)
        {
            try
            {
                var findCategory = await _categoryRepository.FindAsync(ca => ca.Name == req.Name);
                if (findCategory != null)
                {
                    return new CategoryDTO
                    {
                        IsSuccess = false,
                        ErrorMessage = "Category has exists",
                    };
                }
                await _unitOfWork.BeginTransaction(); 
                var categories = new Category
                {
                    ParentId = req.ParentId,
                    ShopId = req.ShopId,
                    Name = req.Name,
                    Description = req.Description,
                    Gender = req.Gender,
                };   
                // await _categoryRepository.AddAsync(categories);
                var img = new Image{
                    Path = req.ImagePath,
                    CategoryId = categories.Id
                };
                categories.Image = img;
                await _categoryRepository.AddAsync(categories);
                await _unitOfWork.CommitTransaction();
                    return new CategoryDTO
                    {
                        IsSuccess = true,
                        ErrorMessage = "Add category success",
                    };            }
            catch (Exception ex)
            {
                ex = new Exception(ex.Message);
                throw ex;
            }
        }

        public async Task<CategoryResponse> AddParentCategory(CategoryRequest req)
        {
            try
            {
                var findCategory = await _categoryRepository.FindAsync(ca => ca.Name == req.Name);
                if (findCategory != null)
                {
                    return new CategoryResponse{
                        IsSuccess = false,
                        ErrorMessage = "Category not found!!"
                    };
                }
                await _unitOfWork.BeginTransaction(); 
                var categories = new Category
                {
                    Name = req.Name,
                    Description = req.Description,
                    Gender = req.Gender,
                    // Image = req.ImagePath,
                };  
                var img = new Image{
                    Path = req.ImagePath,
                    CategoryId = categories.Id
                };
                categories.Image = img;
                await _categoryRepository.AddAsync(categories);
                await _unitOfWork.CommitTransaction();
                return new CategoryResponse{
                    IsSuccess = true,
                    ErrorMessage = "Add Category Success!!"
                };       
            }
            catch (Exception ex)
            {
                return new CategoryResponse{
                    IsSuccess = false,
                    ErrorMessage = ex.Message,
                };
            }
        }

        public async Task<List<CategoryDTO>> GetAllCategpry()
        {
            var listCategoryDTO = await _categoryRepository.GetAll();
            var categoryDTO = new List<CategoryDTO>();

            foreach (var item in listCategoryDTO)
            {
                if((item.ParentId == null)&&(item.ShopId==null)){
                    var category = new CategoryDTO()
                    {
                        Id = item.Id,
                        ParentId = item.ParentId,
                        ShopId = item.ShopId,
                        Name = item.Name,
                        Gender = item.Gender,
                        Description = item.Description,
                        ImagePath = item.Image.Path,
                    };
                    categoryDTO.Add(category);
                }   
            }
            return categoryDTO;
        }

        public async Task<List<CategoryDTO>> GetCategoryByParentId(int idCategory)
        {
            var category = await _categoryRepository.ListCategory(idCategory);
            var listCategories = _mapper.MapCategories(category);
            var parentId = await _categoryRepository.GetCategory(idCategory);
            foreach (var listCategory in listCategories)
            {
                listCategory.NameParent = parentId.Name;  
            }
            return listCategories;
        }

        public async Task<CategoryDTO>  GetCategory(int idCategory)
        {
            // 1. Find category
            var category = await _categoryRepository.FindAsync(p => p.Id == idCategory);
            if (category == null)
            {
                return new CategoryDTO
                {
                    IsSuccess = false,
                    ErrorMessage = "Category not found!!!!!!!",
                };
            }

            var listCategoryDTO = new List<CategoryDTO>();
            var listItemsDTO = new List<ItemDTO>();

            // 2. Check if category parent
            if(category.ParentId == null)
            {
                // 3. Get all info child category
                listCategoryDTO = await GetCategoryByParentId(category.Id);

                foreach (var categoryDTO in listCategoryDTO)
                {
                    // 4. Save items
                    listItemsDTO.AddRange(categoryDTO.Items);
                }

                // 5. Return information of parent category
                return new CategoryDTO
                {
                    IsSuccess = true,
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                    Gender  = category.Gender,
                    Items = listItemsDTO,
                };
            }
            var parentId = await _categoryRepository.GetCategory(category.ParentId.Value);

            // 6. Return all information of child category
            return new CategoryDTO
            {
                IsSuccess = true,
                Id = category.Id,
                ParentId = category.ParentId,
                Name = category.Name,
                NameParent = parentId.Name,
                Description = category.Description,
                Items = _mapper.MapItems(category.Items.ToList()),
            };
        }

        public async Task<CategoryResponse> RemoveParentCategory(int idCategory)
        {
            try
            {
                var findParent = await _categoryRepository.ListCategory(idCategory);
                var findCategory = await _categoryRepository.FindAsync(it => it.Id == idCategory);
                if(findCategory == null)
                {

                    return new CategoryResponse{
                        IsSuccess = false,
                        ErrorMessage = "Category not found!!"
                    };
                    // throw new Exception("Item not found!!");
                }
                await _unitOfWork.BeginTransaction();
                _categoryRepository.Delete(findCategory);
                foreach (var category in findParent)
                {
                    category.ParentId = null;
                }
                await _unitOfWork.CommitTransaction();
                
                return new CategoryResponse
                {
                    IsSuccess = true,
                };
                
            }
            catch (Exception ex)
            {
                return new CategoryResponse
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message,
                };
            }
        }

        public async Task<CategoryResponse> UpdateCategoryByCategoryId(CategoryRequest req)
        {
            try
            {
                var categoryReq = await _categoryRepository.FindAsync(it => it.Id == req.Id);
                if(categoryReq == null)
                {
                    return new CategoryResponse
                    {
                        IsSuccess = false,
                        ErrorMessage = "Category not found!!"
                    };
                }
                await _unitOfWork.BeginTransaction();
                categoryReq.ParentId = req.ParentId;
                categoryReq.ShopId = req.ShopId;
                categoryReq.Name = req.Name;
                categoryReq.Description = req.Description;
                categoryReq.Image.Path = req.ImagePath;
                _categoryRepository.Update(categoryReq);
                await _unitOfWork.CommitTransaction();
                return new CategoryResponse{
                    IsSuccess = true,
                };
            }
            catch (Exception ex)
            {
                return new CategoryResponse
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message,
                };
            }
        }
    }
}