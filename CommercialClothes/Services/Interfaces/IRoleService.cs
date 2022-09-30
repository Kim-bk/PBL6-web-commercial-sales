using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommercialClothes.Models.DTOs.Requests;

namespace CommercialClothes.Services.Interfaces
{
    public interface IRoleService
    {
        Task<bool> CreateRole(string roleName);
        Task<bool> UpdateRole(RoleRequest req);
        Task<bool> DeleteRole(int roleId);
        Task<bool> AddUserGroup(string userGroupName);
        Task<bool> DeleteUserGroup(int userGroupId);
        Task<bool> AddCredential(CredentialRequest req);
        Task<bool> RemoveCredential(CredentialRequest req);
    }
}
