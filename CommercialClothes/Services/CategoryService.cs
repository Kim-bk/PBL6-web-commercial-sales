using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommercialClothes.Models;
using CommercialClothes.Models.DAL;
using CommercialClothes.Models.DAL.Repositories;
using CommercialClothes.Models.DTOs.Requests;
using CommercialClothes.Models.DTOs.Responses;
using CommercialClothes.Services.Base;
using CommercialClothes.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CommercialClothes.Services
{
    public class CategoryService : BaseService, ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapperCustom _mapper;
        public CategoryService(ICategoryRepository categoryRepository ,IUnitOfWork unitOfWork,IMapperCustom mapper) : base(unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
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
                };   
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
            var listcategory = await _categoryRepository.GetAll();
            var categoryDTO = new List<CategoryDTO>();

            foreach (var item in listcategory)
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
                    };
                    categoryDTO.Add(category);
                }   
            }
            return categoryDTO;
        }

        public async Task<List<CategoryDTO>> GetCategoryByParentId(int idCategory)
        {
            var category = await _categoryRepository.ListCategory(idCategory);
            return _mapper.MapCategories(category);
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

        public List<ItemDTO> GetItemByCategory(List<Item> items)
        {
            return _mapper.MapItems(items);
        }

        public async Task<List<CategoryDTO>> GetItemByCategoryId(int idCategory)
        {
            var item = await _categoryRepository.FindAsync(p => p.Id == idCategory);
            var itemByCategoryId = new List<CategoryDTO>();
            if (item == null)
            {
                throw new Exception("Item not found!!!!!!!");
            }
            var items = new CategoryDTO()
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Items = GetItemByCategory(item.Items.ToList()),
            };
            itemByCategoryId.Add(items);
            return itemByCategoryId;  
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