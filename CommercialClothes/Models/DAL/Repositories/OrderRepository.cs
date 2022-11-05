using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommercialClothes.Models;
using CommercialClothes.Models.DAL;
using CommercialClothes.Models.DAL.Repositories;

namespace PBL6.pbl6_web_commercial_sales.CommercialClothes.Models.DAL.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }

        public List<Order> GetOrders(int userId)
        {
            return GetQuery(ord => ord.AccountId == userId).ToList();
        }
    }
}