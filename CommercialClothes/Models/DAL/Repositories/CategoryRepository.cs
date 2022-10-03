using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommercialClothes.Models.DTOs.Responses;
using Microsoft.EntityFrameworkCore;

namespace CommercialClothes.Models.DAL.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }

        public async Task<List<Category>> ListCategory(int parentId)
        {
             return await DbSet.Where(it => it.ParentId == parentId).ToListAsync();
        }
    }
}