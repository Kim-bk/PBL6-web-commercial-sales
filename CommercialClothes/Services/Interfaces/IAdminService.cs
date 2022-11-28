using CommercialClothes.Models.DTOs.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommercialClothes.Services.Interfaces
{
    public interface IAdminService
    {
        Task<List<CredentialResponse>> GetRolesOfUserGroup(int userGroup);
    }
}
