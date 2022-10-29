using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommercialClothes.Models;
using CommercialClothes.Models.DAL;
using CommercialClothes.Models.DAL.Repositories;
using CommercialClothes.Models.DTOs;
using CommercialClothes.Models.DTOs.Requests;
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

        public async Task<bool> AddCategory(CategoryRequest req)
        {
            try
            {
                var findCategory = await _categoryRepository.FindAsync(ca => ca.Name == req.Name);
                if (findCategory != null)
                {
                    throw new Exception("Category is already existed!");
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
                return true;         
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<bool> AddParentCategory(CategoryRequest req)
        {
            try
            {
                var findCategory = await _categoryRepository.FindAsync(ca => ca.Name == req.Name);
                if (findCategory != null)
                {
                    throw new Exception("Category is already existed!");
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
                return true;         
            }
            catch (Exception e)
            {
                throw e;
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

        public List<ImageDTO> GetImages(List<Image> images)
        {
            var listImageDTO = new List<ImageDTO>();
            foreach (var item in images)
            {
                var imageDTO = new ImageDTO()
                {
                    // ShopId = item.ShopId,
                    Path = item.Path,
                };
                listImageDTO.Add(imageDTO);
            }
            return listImageDTO;
        }

        public async Task<List<CategoryDTO>> GetItem(int parentId)
        {
            var category = await _categoryRepository.ListCategory(parentId);
            return _mapper.MapCategoriesGetItem(category);
        }

        public List<ItemDTO> GetItemByCategory(List<Item> items)
        {
            return _mapper.MapItems(items);
        }

        public async Task<CategoryDTO> GetCategory(int idCategory)
        {
            // 1. Find category
            var category = await _categoryRepository.FindAsync(p => p.Id == idCategory);
            if (category == null)
            {
                throw new Exception("Category not found!!!!!!!");
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
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                    Gender  = category.Gender,
                    Items = listItemsDTO,
                };
            }
            var parentId = await _categoryRepository.GetCategory(category.ParentId.Value);
            // 5. Return all information of child category
            return new CategoryDTO
            {
                Id = category.Id,
                ParentId = category.ParentId,
                Name = category.Name,
                NameParent = parentId.Name,
                Description = category.Description,
                Items = _mapper.MapItems(category.Items.ToList()),
            };
        }

        public async Task<bool> RemoveParentCategory(int idCategory)
        {
            try
            {
                var findParent = await _categoryRepository.ListCategory(idCategory);
                var findCategory = await _categoryRepository.FindAsync(it => it.Id == idCategory);
                if((findCategory == null))
                {
                    throw new Exception("Item not found!!");
                }
                await _unitOfWork.BeginTransaction();
                _categoryRepository.Delete(findCategory);
                foreach (var category in findParent)
                {
                    category.ParentId = null;
                }
                await _unitOfWork.CommitTransaction();
                return true;
                
            }
            catch (Exception e)
            {
                throw e; 
            }
        }

        public async Task<bool> UpdateCategoryByCategoryId(CategoryRequest req)
        {
            try
            {
                var categoryReq = await _categoryRepository.FindAsync(it => it.Id == req.Id);
                if(categoryReq == null)
                {
                    throw new Exception("Item not found!!");
                }
                await _unitOfWork.BeginTransaction();
                categoryReq.ParentId = req.ParentId;
                categoryReq.ShopId = req.ShopId;
                categoryReq.Name = req.Name;
                categoryReq.Description = req.Description;
                categoryReq.Image.Path = req.ImagePath;
                _categoryRepository.Update(categoryReq);
                await _unitOfWork.CommitTransaction();
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}