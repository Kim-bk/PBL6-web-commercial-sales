using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommercialClothes.Models.DAL.Interfaces;

namespace CommercialClothes.Models.DAL.Repositories
{
    public class StatisticalRepository : Repository<Item>, IStatisticalRepository
    {
        public StatisticalRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}