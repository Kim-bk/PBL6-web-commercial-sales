using System;
using System.Threading.Tasks;
using ComercialClothes.Models.DTOs.Requests;
using CommercialClothes.Models;
using CommercialClothes.Models.DTOs.Requests;

namespace CommercialClothes.Services
{
    public interface IUserService
    {
        Task<Account> Login(LoginRequest req);
        Task<bool> Register(RegistRequest req);
        Task<bool> UpdateUser(UserRequest req);
        Task<bool> CheckUserByActivationCode(Guid activationCode);
        Task<bool> ResetPassword(ResetPasswordRequest request);
        Task<bool> ForgotPassword(string userEmail);
        Task<bool> GetUserByResetCode(Guid resetPassCode);
        Task<Account> FindById(int userId);
        Task<bool> Logout(int userId);
    }
}
