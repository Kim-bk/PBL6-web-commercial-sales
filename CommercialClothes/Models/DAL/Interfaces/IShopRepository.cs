using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComercialClothes.Models;

namespace CommercialClothes.Models.DAL.Interfaces
{
    public interface IShopRepository : IRepository<Shop>
    {
        public Task<List<Shop>> SearchShopByName(string keyword); 
    }
}
