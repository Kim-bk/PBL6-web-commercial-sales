using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using CommercialClothes.Models;
using CommercialClothes.Models.DAL;
using CommercialClothes.Models.DAL.Interfaces;
using CommercialClothes.Models.DAL.Repositories;
using CommercialClothes.Models.DTOs.Requests;
using CommercialClothes.Models.DTOs.Responses;
using CommercialClothes.Models.DTOs.Responses.Base;
using CommercialClothes.Services.Base;
using CommercialClothes.Services.Interfaces;

namespace CommercialClothes.Services
{
    public class PermissionService : BaseService, IPermissionService
    {
        private readonly ICredentialRepository _credentialRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserGroupRepository _userGroupRepository;
        private readonly IUserRepository _userRepository;
       
        public PermissionService(IUnitOfWork unitOfWork, IMapperCustom mapper
                , ICredentialRepository credentialRepository, IRoleRepository roleRepository
                , IUserGroupRepository userGroupRepository, IUserRepository userRepository) : base(unitOfWork, mapper)
        {
            _credentialRepository = credentialRepository;
            _roleRepository = roleRepository;
            _userGroupRepository = userGroupRepository;
            _userRepository = userRepository;
        }
        public async Task<string> GetCredentials(int userId)
        {
            // 1. Get User Group Id of user
            var groupUserId = (await _userRepository.FindAsync(us => us.Id == userId)).UserGroupId;

            // 2. Get credentials of user
            List<string> listCredentials = await _credentialRepository.GetCredentialsByUserGroupId(groupUserId.Value);
            string combinedString = string.Join(",", listCredentials.ToArray());
            return combinedString;
        }

        public async Task<GeneralResponse> AddCredential(CredentialRequest req)
        {
            try
            {
                // 1. Check duplicate
                var existCredential = await _credentialRepository.FindAsync
                                (c => c.RoleId == req.RoleId && c.UserGroupId == req.UserGroupId);
                if (existCredential != null)
                {
                    return new GeneralResponse
                    {
                        IsSuccess = false,
                        ErrorMessage = "Vai trò đã được phân quyền !",
                    };
                }

                // 2. Find role
                var role = await _roleRepository.FindAsync(r => r.Id == req.RoleId);

                // 3. Find user group
                var userGroup = await _userGroupRepository.FindAsync(us => us.Id == req.UserGroupId);

                // 4. Add new Credential
                var newCredential = new Credential
                {
                    Role = role,
                    UserGroup = userGroup,
                };
                await _credentialRepository.AddAsync(newCredential);
                await _unitOfWork.CommitTransaction();
                return new GeneralResponse
                {
                    IsSuccess = true
                };
            }
            catch (Exception e)
            {
                return new GeneralResponse
                {
                    IsSuccess = false,
                    ErrorMessage = e.Message
                };
            }
        }   

        public async Task<GeneralResponse> RemoveCredential(CredentialRequest req)
        {
            try
            {
                // 1. Check credential
                var existedCredential = await _credentialRepository.FindAsync
                    (c => c.RoleId == req.RoleId && c.UserGroupId == req.UserGroupId);
                if (existedCredential == null)
                {
                    return new GeneralResponse
                    { 
                        IsSuccess = false,
                        ErrorMessage = "Đầu vào không hợp lệ !"
                    };
                }

                // 2. Else delete that credential
                 _credentialRepository.Delete(existedCredential);
                await _unitOfWork.CommitTransaction();
                return new GeneralResponse
                {
                    IsSuccess = true
                };
            }
            catch (Exception e)
            {
                return new GeneralResponse
                {
                    IsSuccess = false,
                    ErrorMessage = e.Message
                };
            }
        }       
    }
}
