using CommercialClothes.Models.DAL;
using CommercialClothes.Models.DAL.Interfaces;
using CommercialClothes.Models.DTOs.Responses;
using CommercialClothes.Services.Base;
using CommercialClothes.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommercialClothes.Services
{
    public class AdminService : BaseService, IAdminService
    {
        private readonly ICredentialRepository _credentialRepo;
        private readonly IRoleRepository _roleRepo;
        public AdminService(ICredentialRepository credentialRepo, IMapperCustom mapper
            , IUnitOfWork unitOfWork, IRoleRepository roleRepo) : base(unitOfWork, mapper)
        {
            _credentialRepo = credentialRepo;
            _roleRepo = roleRepo;
        }

        public async Task<List<CredentialResponse>> GetRolesOfUserGroup(int userGroup)
        {
            var listCredentials = new List<CredentialResponse>();
            var rolesActivated = await _credentialRepo.GetRolesOfUserGroup(userGroup);
            var allRoles = await _roleRepo.GetAll();

            foreach (var role in rolesActivated)
            {
                var credential = new CredentialResponse
                {
                    RoleName = role.Role.Description.Trim(),
                    RoleId = role.RoleId,
                    IsActivated = role.IsActivated,
                };
                listCredentials.Add(credential);
            }    

            foreach (var role in allRoles)
            {
                if (listCredentials.FirstOrDefault(r => r.RoleId == role.Id) == null)
                {
                    var credential = new CredentialResponse
                    {
                        RoleName = role.Description.Trim(),
                        RoleId = role.Id,
                        IsActivated = false,
                    };
                    listCredentials.Add(credential);
                }    
            }    

            return listCredentials;
        }
    }
}
