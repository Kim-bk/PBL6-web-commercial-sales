using System;
using System.Threading.Tasks;
using ComercialClothes.Models.DTOs.Requests;
using CommercialClothes.Models;
using CommercialClothes.Models.DTOs.Requests;
using CommercialClothes.Models.DTOs.Responses;

namespace CommercialClothes.Services
{
    public interface IUserService
    {
        Task<UserResponse> Login(LoginRequest req);
        Task<UserResponse> Register(RegistRequest req);
        Task<UserResponse> UpdateUser(UserRequest req);
        Task<bool> CheckUserByActivationCode(Guid activationCode);
        Task<UserResponse> ResetPassword(ResetPasswordRequest request);
        Task<UserResponse> ForgotPassword(string userEmail);
        Task<bool> GetUserByResetCode(Guid resetPassCode);
        Task<Account> FindById(int userId);
        Task<bool> Logout(int userId);
    }
}
