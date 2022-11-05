using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommercialClothes.Models.DAL.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        public List<Order> GetOrders(int userId);
    }
}