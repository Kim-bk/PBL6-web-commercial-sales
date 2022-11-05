using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommercialClothes.Models.DTOs;
using CommercialClothes.Models.DTOs.Requests;
using CommercialClothes.Models.DTOs.Responses;
// using Microsoft.AspNetCore.Mvc;

namespace CommercialClothes.Services.Interfaces
{
    public interface ICartService 
    {
        Task<bool> AddCart(CartRequest req, int idAccount);
        Task<List<CartResponse>> GetCartById(int idAccount);
        // List<OrderDetailDTO> GetOrderDetail(int idOrder);
    }
}