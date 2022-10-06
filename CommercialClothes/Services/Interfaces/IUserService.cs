using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComercialClothes.Models;
using ComercialClothes.Models.DTOs.Requests;
using CommercialClothes.Models.DTOs.Requests;

namespace CommercialClothes.Services
{
    public interface IUserService
    {
        Task<bool> Login(LoginRequest req);
        Task<bool> Register(RegistRequest req);
        Task<bool> UpdateUser(UserRequest req);
        Task<bool> CheckUserByActivationCode(Guid activationCode);
        Task<bool> ResetPassword(ResetPasswordRequest request);
        Task<bool> ForgotPassword(string userEmail);
        Task<bool> GetUserByResetCode(Guid resetPassCode);
    }
}
