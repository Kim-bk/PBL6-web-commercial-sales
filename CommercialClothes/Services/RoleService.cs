using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComercialClothes.Models;
using CommercialClothes.Models;
using CommercialClothes.Models.DAL;
using CommercialClothes.Models.DAL.Interfaces;
using CommercialClothes.Models.DTOs.Requests;
using CommercialClothes.Services.Base;
using CommercialClothes.Services.Interfaces;

namespace CommercialClothes.Services
{
    public class RoleService : BaseService, IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUserGroupRepository _userGroupRepository;
        public RoleService(IRoleRepository roleRepository, IUnitOfWork unitOfWork
            , IUserGroupRepository userGroupRepository, IMapperCustom mapper) : base(unitOfWork, mapper)
        {
            _roleRepository = roleRepository;
            _userGroupRepository = userGroupRepository;
        }

        public Task<bool> AddCredential(CredentialRequest req)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddUserGroup(string userGroupName)
        {
            try 
            {
                // 1. Validate
                if (String.IsNullOrEmpty(userGroupName))
                {
                    throw new ArgumentNullException("Input is empty !");
                }

                var userGroup = await _userGroupRepository.FindAsync(ug => ug.Name == userGroupName);
                if (userGroup != null)
                {
                    throw new ArgumentException("User Group is already existed !");
                }

                var newUserGroup = new UserGroup
                {
                    Name = userGroupName
                };
                await _userGroupRepository.AddAsync(newUserGroup);
                await _unitOfWork.CommitTransaction();
                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> CreateRole(string roleName)
        {
            try
            {
                // 1. Validate 
                if (String.IsNullOrEmpty(roleName))
                {
                    throw new ArgumentNullException("Input is empty !");
                }

                var role = await _roleRepository.FindAsync(r => r.Name == roleName);
                if (role != null)
                {
                    throw new ArgumentException("Role is already existed !");
                }

                var newRole = new Role { Name = roleName };
                await _roleRepository.AddAsync(newRole);
                await _unitOfWork.CommitTransaction();
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<bool> DeleteRole(int roleId)
        {
            try
            {
                _roleRepository.DeleteExp(r => r.Id == roleId);
                await _unitOfWork.CommitTransaction();
                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> DeleteUserGroup(int userGroupId)
        {
            try
            {
                _userGroupRepository.DeleteExp(ug => ug.Id == userGroupId);
                await _unitOfWork.CommitTransaction();
                return true;
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public Task<bool> RemoveCredential(CredentialRequest req)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateRole(RoleRequest req)
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
                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
