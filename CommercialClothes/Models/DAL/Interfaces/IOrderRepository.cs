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
        Task<List<Order>> GetOrdersCancelByDate(string dateTime, int idShop);
        List<Order> GetOrders(int userId);
        Task<List<Order>> GetOrdersByShop(int idShop);
    }
}