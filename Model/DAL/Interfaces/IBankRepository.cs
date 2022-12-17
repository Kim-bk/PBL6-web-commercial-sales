using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommercialClothes.Models.Entities;

namespace CommercialClothes.Models.DAL.Interfaces
{
    public interface IBankRepository : IRepository<Bank>
    {
        Task<List<Bank>> GetUserBanks(int accountId);
    }
}