using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComercialClothes.Models;
using CommercialClothes.Models.DTOs;

namespace CommercialClothes.Models.DAL.Interfaces
{
    public interface IItemRepository : IRepository<Item>
    {
        Task<List<Item>> GetItemById(int idItem);
        Task<List<Item>> SearchItem(string keyword);
    }
}
