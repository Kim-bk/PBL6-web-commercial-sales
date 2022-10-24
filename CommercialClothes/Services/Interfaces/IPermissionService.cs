using System.Threading.Tasks;
using CommercialClothes.Models.DTOs.Requests;
using CommercialClothes.Models.DTOs.Responses;
using CommercialClothes.Models.DTOs.Responses.Base;

namespace CommercialClothes.Services.Interfaces
{
    public interface IPermissionService
    {
        Task<GeneralResponse> CreateRole(string roleName);
        Task<GeneralResponse> UpdateRole(RoleRequest req);
        Task<GeneralResponse> DeleteRole(int roleId);
        Task<GeneralResponse> AddUserGroup(string userGroupName);
        Task<GeneralResponse> DeleteUserGroup(int userGroupId);
        Task<GeneralResponse> UpdateUserGroup(UserGroupRequest req);
        Task<GeneralResponse> AddCredential(CredentialRequest req);
        Task<GeneralResponse> RemoveCredential(CredentialRequest req);
    }
}
