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
        // public Task<bool> Save(HistoryTransaction transaction);
    }
}