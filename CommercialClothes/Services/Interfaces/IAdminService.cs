using ComercialClothes.Models.DTOs.Requests;
using CommercialClothes.Models.DTOs.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommercialClothes.Services.Interfaces
{
    public interface IAdminService
    {
        Task<List<CredentialResponse>> GetRolesOfUserGroup(int userGroup);
        Task<UserResponse> Login(LoginRequest request);
    }
}
