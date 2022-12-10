using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommercialClothes.Models.DTOs;
using CommercialClothes.Models.DTOs.Requests;
using CommercialClothes.Models.DTOs.Responses;

namespace CommercialClothes.Services.Interfaces
{
    public interface IBankService
    {
        Task<BankResponse> AddBank(BankRequest req, int idAccount);
        Task<BankResponse> UpdateBank(BankRequest req, int idAccount);
        Task<List<BankDTO>> GetBankById(int idAccount);
        Task<BankResponse> GetBanksByUser(int userId);
    }
}