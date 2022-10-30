using CommercialClothes.Models.DTOs.Requests;
using CommercialClothes.Models.DTOs.Responses.Base;
using System.Threading.Tasks;

namespace CommercialClothes.Services.Interfaces
{
    public interface IRoleService
    {
        Task<GeneralResponse> CreateRole(string roleName);
        Task<GeneralResponse> UpdateRole(RoleRequest req);
        Task<GeneralResponse> DeleteRole(int roleId);
    }
}
