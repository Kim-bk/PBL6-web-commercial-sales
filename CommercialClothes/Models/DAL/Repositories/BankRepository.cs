using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommercialClothes.Models.DAL.Interfaces;
using CommercialClothes.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CommercialClothes.Models.DAL.Repositories
{
    public class BankRepository : Repository<Bank>, IBankRepository
    {
        public BankRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }

        public async Task<List<Bank>> ListBank(int accountId)
        {
            return await GetQuery(bk => bk.AccountId == accountId).ToListAsync();
        }
    }
}