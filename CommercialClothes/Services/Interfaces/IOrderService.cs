using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommercialClothes.Models.DTOs;
using CommercialClothes.Models.DTOs.Requests;
using CommercialClothes.Models.DTOs.Responses;

namespace CommercialClothes.Services.Interfaces
{
    public interface IOrderService
    {
        Task<string> AddOrder(OrderRequest req, int userId);
        Task<StatusResponse> UpdateStatusOrder(StatusRequest req,int orderId);
        Task<bool> CancelOrder(int orderId);
        Task<OrderDetailResponse> GetOrderDetails(int orderId);

    }
}