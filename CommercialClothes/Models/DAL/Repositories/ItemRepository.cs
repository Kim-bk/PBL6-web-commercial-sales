using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComercialClothes.Models.DAL.Repositories
{
    public class ItemRepository : Repository<Item>, IItemRepository
    {
        public ItemRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }

        public async Task<List<Item>> GetAllItem()
        {
            return await DbSet.ToList();
        }
    }
}