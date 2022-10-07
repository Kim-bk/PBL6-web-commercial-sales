using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComercialClothes.Models;
using CommercialClothes.Models.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CommercialClothes.Models.DAL.Repositories
{
    public class ShopRepository : Repository<Shop>, IShopRepository
    {
        public ShopRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }
        public async Task<List<Shop>> SearchShopByName(string keyword)
        {
            return await DbSet.Where(s => s.Name.ToLower().Contains(keyword.ToLower())).ToListAsync();
        }
    }
}
