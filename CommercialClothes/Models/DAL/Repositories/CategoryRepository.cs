using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CommercialClothes.Models.DAL.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }

        public async Task<Category> GetCategory(int categoryId)
        {
            return await GetQuery(it => it.Id == categoryId).FirstAsync();
        }

        public async Task<List<Category>> ListCategory(int parentId)
        {
            return await GetQuery(it => it.ParentId == parentId).ToListAsync();
            //return await DbSet.Where(it => it.ParentId == parentId).ToListAsync();
        }
    }
}