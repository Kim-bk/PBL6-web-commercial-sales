using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
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

        public async Task<List<Order>> GetCart(int userId)
        {
            return await GetQuery(ord => ord.AccountId == userId && ord.IsBought == false).ToListAsync();
        }
        public async Task<List<Order>>  GetOrdersByDate(string dateTime, int idShop) 
        {
            if(dateTime.Length == 10)
            {
                return await GetQuery(or => or.DateCreate.ToString().Substring(0,10).Equals(dateTime) && or.IsBought == true && or.StatusId != 4 && or.ShopId == idShop).ToListAsync();
            }
            if(dateTime.Length == 13)
            {
                return await GetQuery(or => or.DateCreate.ToString().Substring(0,13).Equals(dateTime) && or.IsBought == true && or.StatusId != 4 && or.ShopId == idShop).ToListAsync();
            }
            return null;
        }
        
        public List<Order> GetOrders(int userId)
        {
            return GetQuery(ord => ord.AccountId == userId && ord.IsBought == false).ToList();
        }

        public List<Order> ViewHistoriesOrder(int userId)
        {
            return GetQuery(ord => ord.AccountId == userId && ord.IsBought == true).ToList();
        }
        public async Task<List<Order>> GetOrdersByShop(int idShop)
        {
            return await GetQuery(ord => ord.ShopId == idShop).ToListAsync();
        }

        public async Task<List<Order>> GetOrdersCancelByDate(string dateTime, int idShop)
        {
            if(dateTime.Length == 10)
            {
                return await GetQuery(or => or.DateCreate.ToString().Substring(0,10).Equals(dateTime) && or.IsBought == true && or.StatusId == 4 && or.ShopId == idShop).ToListAsync();
            }
            if(dateTime.Length == 13)
            {
                return await GetQuery(or => or.DateCreate.ToString().Substring(0,13).Equals(dateTime) && or.IsBought == true && or.StatusId != 4 && or.ShopId == idShop).ToListAsync();
            }
            return null;
        }
    }
}