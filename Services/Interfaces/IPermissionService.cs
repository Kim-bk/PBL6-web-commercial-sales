using System.Collections.Generic;
using System.Threading.Tasks;
using CommercialClothes.Models.DTOs.Requests;
using CommercialClothes.Models.DTOs.Responses.Base;

namespace CommercialClothes.Services.Interfaces
{
    public interface IPermissionService
    {
        Task<string> GetCredentials(int userId);
        Task<GeneralResponse> AddCredential(CredentialRequest req);
        Task<GeneralResponse> RemoveCredential(CredentialRequest req);
    }
}
