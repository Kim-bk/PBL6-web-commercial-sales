﻿using ComercialClothes.Models.DTOs.Requests;
using CommercialClothes.Models.DTOs;
using CommercialClothes.Models.DTOs.Responses;
using Model.DTOs;
using Model.DTOs.Requests;
using Model.DTOs.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommercialClothes.Services.Interfaces
{
    public interface IAdminService
    {
        public Task<List<CredentialResponse>> GetRolesOfUserGroup(int userGroup);

        public Task<UserResponse> Login(LoginRequest request);

        public Task<List<UserDTO>> GetUsers();

        public Task<bool> UpdateUserGroupOfUser(UserGroupUpdatedRequest request);

        public Task<bool> ManageTransaction(TransactionDTO transactionDto, int userGroupId);

        public Task<List<TransactionResponse>> GetTransactions();
    }
}