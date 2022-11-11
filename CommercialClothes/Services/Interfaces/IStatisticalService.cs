using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommercialClothes.Models.DTOs;

namespace CommercialClothes.Services
{
    public interface IStatisticalService
    {
        Task<List<StatisticalDTO>> ListItemsSold(int idShop);

        Task<List<StatisticalDTO>> ListItemsSoldByDate(int idShop, string dateTime);
    }
}