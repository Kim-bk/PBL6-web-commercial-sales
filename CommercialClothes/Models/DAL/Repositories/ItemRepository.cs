using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommercialClothes.Models.DTOs.Responses;
using Microsoft.EntityFrameworkCore;

namespace ComercialClothes.Models.DAL.Repositories
{
    public class ItemRepository : Repository<Item>, IItemRepository
    {
        public ItemRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }

        // public async Task<List<Image>> GetImagesByItemId(int idItem)
        // {
        //     return await DbSet.Where(img => img.)
        // }

        public async Task<List<Item>> GetItemById(int idItem)
        {
            return await DbSet.Where(it => it.Id == idItem).ToListAsync();
        }
        
    }
}