using CommercialClothes.Models.DTOs.Requests;
using CommercialClothes.Models.DTOs.Responses;
using CommercialClothes.Models.DTOs.Responses.Base;
using System.Threading.Tasks;

namespace CommercialClothes.Services.Interfaces
{
    public interface IUserGroupService
    {
        Task<UserGroupResponse> GetUserGroups();
        Task<GeneralResponse> AddUserGroup(string userGroupName);
        Task<GeneralResponse> DeleteUserGroup(int userGroupId);
        Task<GeneralResponse> UpdateUserGroup(UserGroupRequest req);
    }
}
