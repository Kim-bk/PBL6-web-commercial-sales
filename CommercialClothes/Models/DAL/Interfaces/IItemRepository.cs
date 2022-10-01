using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommercialClothes.Models.DTOs.Responses;

namespace CommercialClothes.Models.DAL.Repositories
{
    public interface IItemRepository : IRepository<Item>
    {
        Task<List<Item>> GetItemById(int idItem);
    }
}
