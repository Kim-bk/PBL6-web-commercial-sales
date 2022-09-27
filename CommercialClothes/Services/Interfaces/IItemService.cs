using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommercialClothes.Models.DTOs;

namespace CommercialClothes.Services.Interfaces
{
    public interface IItemService
    {
        Task<List<ItemDTO>> SearchItem(string searchContent);
    }
}
