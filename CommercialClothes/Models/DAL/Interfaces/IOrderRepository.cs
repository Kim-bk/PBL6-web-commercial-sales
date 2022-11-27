using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommercialClothes.Models.DAL.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        public List<Order> ViewHistoriesOrder(int userId);
        Task<List<Order>> GetCart(int userId);
        Task<List<Order>> GetOrdersByDate(string dateTime, int idShop);
        // Task<List<Order>> GetOrdersByMonth(string month);
        public List<Order> GetOrders(int userId);
        Task<List<Order>> GetOrdersByInterval(string startDate, string endDate);


    }
}