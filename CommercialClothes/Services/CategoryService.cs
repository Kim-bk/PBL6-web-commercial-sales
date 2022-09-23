using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComercialClothes.Models;
using ComercialClothes.Models.DAL;
using ComercialClothes.Models.DAL.Repositories;
using CommercialClothes.Models.DTOs.Responses;
using CommercialClothes.Services.Base;
using CommercialClothes.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CommercialClothes.Services
{
    public class CategoryService : BaseService, ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository ,IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _categoryRepository = categoryRepository;
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

        public List<ItemDTO> GetItemByCategory(List<Item> items)
        {
            var listItemDTO = new List<ItemDTO>();
            foreach (var item in items)
            {
                var itemDTO = new ItemDTO()
                {
                    Id = item.Id,
                    CategoryId = item.CategoryId,
                    Name = item.Name,
                    Price = item.Price,
                    Description = item.Description,
                    DateCreated = item.DateCreated,
                    Quantity = item.Quantity,
                    Size = item.Size,
                    // Images = GetImages(item,Images.)
                };
                listItemDTO.Add(itemDTO);
            }
            return listItemDTO;
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
    }
}