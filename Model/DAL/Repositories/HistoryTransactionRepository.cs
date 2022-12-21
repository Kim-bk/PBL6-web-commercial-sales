using CommercialClothes.Models.DAL;
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
    }
}