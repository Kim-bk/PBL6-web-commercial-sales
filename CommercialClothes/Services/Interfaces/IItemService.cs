using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComercialClothes.Models;
using ComercialClothes.Models.DAL.Repositories;
using CommercialClothes.Models.DTOs.Responses;
using Microsoft.AspNetCore.Mvc;

namespace CommercialClothes.Services.Interfaces
{
    public interface IItemService
    {
        Task<List<ItemDTO>> GetAllItem();
        List<ImageDTO> GetImages(List<Image> images);
        Task<List<ItemDTO>> GetItembyID(int id);
        // Task<List<ItemIdDTO>> GetItembyCategory(int idcategory);

    }
}