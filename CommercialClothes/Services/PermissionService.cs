using System;
using System.Threading.Tasks;
using CommercialClothes.Models;
using CommercialClothes.Models.DAL;
using CommercialClothes.Models.DAL.Interfaces;
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
        public PermissionService(IRoleRepository roleRepository, IUnitOfWork unitOfWork
                , IUserGroupRepository userGroupRepository, IMapperCustom mapper
                , ICredentialRepository credentialRepository) : base(unitOfWork, mapper)
        {
            _roleRepository = roleRepository;
            _userGroupRepository = userGroupRepository;
            _credentialRepository = credentialRepository;
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

                // 2. Add new Credential
                var newCredential = new Credential
                {
                    RoleId = req.RoleId,
                    UserGroupId = req.UserGroupId,
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
        public async Task<GeneralResponse> AddUserGroup(string userGroupName)
        {
            try 
            {
                // 1. Validate
                if (String.IsNullOrEmpty(userGroupName))
                {
                    return new GeneralResponse
                    {
                        IsSuccess = false,
                        ErrorMessage = "Cú pháp không hợp lệ !"
                    };
                }

                var userGroup = await _userGroupRepository.FindAsync(ug => ug.Name == userGroupName);
                if (userGroup != null)
                {
                    return new GeneralResponse
                    {
                        IsSuccess = false,
                        ErrorMessage = "User Group đã tồn tại !"
                    };
                }

                var newUserGroup = new UserGroup
                {
                    Name = userGroupName
                };
                await _userGroupRepository.AddAsync(newUserGroup);
                await _unitOfWork.CommitTransaction();
                return new GeneralResponse
                {
                    IsSuccess = true,
                };
            }
            catch (Exception e)
            {
                return new GeneralResponse
                {
                    IsSuccess = false,
                    ErrorMessage = e.Message,
                };
            }
        }
        public async Task<GeneralResponse> CreateRole(string roleName)
        {
            try
            {
                // 1. Validate 
                if (String.IsNullOrEmpty(roleName))
                {
                    return new GeneralResponse
                    {
                        IsSuccess = false,
                        ErrorMessage = "Cú pháp không hợp lệ !"
                    };
                }

                var role = await _roleRepository.FindAsync(r => r.Name == roleName);
                if (role != null)
                {
                    return new GeneralResponse
                    {
                        IsSuccess = false,
                        ErrorMessage = "Vai trò đã tồn tại !"
                    };
                }

                var newRole = new Role { Name = roleName };
                await _roleRepository.AddAsync(newRole);
                await _unitOfWork.CommitTransaction();
                return new GeneralResponse
                {
                    IsSuccess = true,
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

        public async Task<GeneralResponse> DeleteRole(int roleId)
        {
            try
            {
                await _roleRepository.Delete(r => r.Id == roleId).ConfigureAwait(false);
                await _unitOfWork.CommitTransaction();
                return new GeneralResponse
                {
                    IsSuccess = true,
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

        public async Task<GeneralResponse> DeleteUserGroup(int userGroupId)
        {
            try
            {
                await _userGroupRepository.Delete(ug => ug.Id == userGroupId);
                await _unitOfWork.CommitTransaction();
                return new GeneralResponse
                {
                    IsSuccess = true,
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
        

        public async Task<GeneralResponse> UpdateRole(RoleRequest req)
        {
            try
            {
                // 1. Find role by Id
                var role = await _roleRepository.FindAsync(r => r.Id == req.RoleId);

                // 2. Check
                if (role == null)
                {
                    throw new ArgumentNullException("Can't find role !");
                }

                // 3. Update
                role.Name = req.Name;
                _roleRepository.Update(role);
                await _unitOfWork.CommitTransaction();
                return new GeneralResponse
                {
                    IsSuccess = true,
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

        public async Task<GeneralResponse> UpdateUserGroup(UserGroupRequest req)
        {
            try
            {
                // 1. Find User Group
                var userGroup = await _userGroupRepository.FindAsync(ug => ug.Id == req.UserGroupId);
                if (userGroup == null)
                {
                    return new GeneralResponse
                    {
                        IsSuccess = false,
                        ErrorMessage = "Không tìm thấy tên quyền " + req.Name + " !"
                    };
                }

                // 2. Update that User Group
                userGroup.Name = req.Name;
                _userGroupRepository.Update(userGroup);
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
