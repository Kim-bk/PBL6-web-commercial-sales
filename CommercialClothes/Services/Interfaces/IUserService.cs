using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComercialClothes.Models.DTOs.Requests;
using CommercialClothes.Models.DTOs.Requests;

namespace CommercialClothes.Services
{
    public interface IUserService
    {
        Task<bool> Login(LoginRequest req);
        Task<bool> Register(RegistRequest req);
        Task<bool> UpdateUser(UserRequest req);
    }
}
