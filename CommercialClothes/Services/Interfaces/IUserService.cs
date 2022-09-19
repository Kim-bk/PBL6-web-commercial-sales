using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComercialClothes.Models;
using ComercialClothes.Models.DTOs.Requests;

namespace CommercialClothes.Services
{
    public interface IUserService
    {
        Task<bool> Login(LoginRequest req);
        Task<bool> Register(RegistRequest req);
        Task<bool> CheckUserByActivationCode(Guid activationCode);
    }
}
