using CommercialClothes.Models;
using CommercialClothes.Models.DAL;
using Microsoft.EntityFrameworkCore;
using Model.DAL.Interfaces;
using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAL.Repositories
{
    public class HistoryTransactionRepository : Repository<HistoryTransaction>, IHistoryTransactionRepository
    {
        public HistoryTransactionRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }

        public async Task<List<HistoryTransaction>> GetTransactionsOfCustomer(int customerId)
        {
            return await GetQuery(s => s.CustomerId == customerId).ToListAsync();
        }

        public async Task<List<HistoryTransaction>> GetTransactionsOfShop(int shopId)
        {
            return await GetQuery(s => s.ShopId == shopId && s.StatusId == 3).ToListAsync();
        }
    }
}