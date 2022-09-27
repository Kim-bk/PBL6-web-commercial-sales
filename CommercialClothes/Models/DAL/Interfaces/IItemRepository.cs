using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComercialClothes.Models;
using ComercialClothes.Models.DAL;
using CommercialClothes.Models.DTOs;

namespace CommercialClothes.Models.DAL.Interfaces
{
    public interface IItemRepository : IRepository<Item>
    {
        Task<List<Item>> SearchItem(string searchContent);
    }
}
