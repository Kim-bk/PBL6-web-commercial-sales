using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ComercialClothes.Models.DTOs.Requests;
using CommercialClothes.Models;
using CommercialClothes.Models.DTOs.Requests;
using CommercialClothes.Models.DTOs.Responses;
using Model.DTOs.Responses;

namespace CommercialClothes.Services
{
    public interface IUserService
    {
        public Task<UserResponse> Login(LoginRequest req);

        public Task<UserResponse> Register(RegistRequest req);

        public Task<UserResponse> UpdateUser(UserRequest req, int idAccount);

        public Task<bool> CheckUserByActivationCode(Guid activationCode);

        public Task<UserResponse> ResetPassword(ResetPasswordRequest request);

        public Task<UserResponse> ForgotPassword(string userEmail);

        public Task<bool> GetUserByResetCode(Guid resetPassCode);

        public Task<UserResponse> FindById(int userId);

        public Task<OrderResponse> GetOrders(int userId);

        public Task<bool> Logout(int userId);

        public Task<List<TransactionResponse>> GetTransactions(int userId);
        public Task<int> GetAccountWallet(int userId);
    }
}