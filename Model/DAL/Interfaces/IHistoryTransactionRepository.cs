using CommercialClothes.Models.DAL;
using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAL.Interfaces
{
    public interface IHistoryTransactionRepository : IRepository<HistoryTransaction>
    {
        public Task<List<HistoryTransaction>> GetTransactionsOfShop(int shopId);

        public Task<List<HistoryTransaction>> GetTransactionsOfCustomer(int customerId);
    }
}