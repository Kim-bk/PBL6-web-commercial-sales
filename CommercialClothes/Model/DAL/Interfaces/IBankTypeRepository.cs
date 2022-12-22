using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommercialClothes.Models.Entities;

namespace CommercialClothes.Models.DAL.Interfaces
{
    public interface IBankTypeRepository : IRepository<BankType>
    {
        Task<List<BankType>> GetAllBanksType();
    }
}