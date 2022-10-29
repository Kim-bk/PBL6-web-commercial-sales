using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComercialClothes.Models;
using CommercialClothes.Models.DTOs;

namespace CommercialClothes.Models.DAL.Repositories
{
    public interface IOrderDetailRepository : IRepository<OrderDetail>
    {
        Task<List<OrderDetail>> ListOrderDetail(int orderId); 
    }
}