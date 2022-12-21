using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommercialClothes.Models.DTOs.Responses;

namespace CommercialClothes.Models.DAL.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<List<Category>> ListCategory(int categoryId); 
        Task<Category> GetCategory(int categoryId);
    }
    
}
