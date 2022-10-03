using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommercialClothes.Models;
using CommercialClothes.Models.DAL;
using CommercialClothes.Models.DAL.Repositories;
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
    }
}