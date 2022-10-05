using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComercialClothes.Models;
using ComercialClothes.Models.DAL;
using CommercialClothes.Models.DAL.Interfaces;

namespace CommercialClothes.Models.DAL.Repositories
{
    public class ShopRepository : Repository<Shop>, IShopRepository
    {
        public ShopRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
