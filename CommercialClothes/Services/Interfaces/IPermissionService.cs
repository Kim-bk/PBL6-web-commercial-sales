using System.Collections.Generic;
using System.Threading.Tasks;
using CommercialClothes.Models.DTOs.Requests;
using CommercialClothes.Models.DTOs.Responses.Base;
using Model.DTOs.Requests;

namespace CommercialClothes.Services.Interfaces
{
    public interface IPermissionService
    {
        public Task<string> GetCredentials(int userId);
        public Task<GeneralResponse> AddCredential(CredentialRequest req);
        public Task<GeneralResponse> RemoveCredential(CredentialRequest req);

        public Task<bool> UpdateCredential(PermissionRequest req);
    }
}
