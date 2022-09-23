using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommercialClothes.Models.DTOs.Responses;

namespace ComercialClothes.Models.DAL.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}