using ComercialClothes.Models.DTOs.Requests;
using CommercialClothes.Models.DAL;
using CommercialClothes.Models.DAL.Interfaces;
using CommercialClothes.Models.DAL.Repositories;
using CommercialClothes.Models.DTOs.Responses;
using CommercialClothes.Services.Base;
using CommercialClothes.Services.Interfaces;
using Org.BouncyCastle.Ocsp;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommercialClothes.Services
{
    public class AdminService : BaseService, IAdminService
    {
        private readonly ICredentialRepository _credentialRepo;
        private readonly IRoleRepository _roleRepo;
        private readonly IUserRepository _userRepo;
        private readonly Encryptor _encryptor;
        public AdminService(ICredentialRepository credentialRepo, IMapperCustom mapper
            , IUnitOfWork unitOfWork, IRoleRepository roleRepo
            , IUserRepository userRepo, Encryptor encryptor) : base(unitOfWork, mapper)
        {
            _credentialRepo = credentialRepo;
            _roleRepo = roleRepo;
            _userRepo = userRepo;
            _encryptor = encryptor;
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

        public async Task<UserResponse> Login(LoginRequest req)
        {
            // 1. Find admin account
            var admin = await _userRepo.FindAsync(us => us.Email == req.Email && us.UserGroupId == 2);

            // 2. Check if user exist
            if (admin == null)
            {
                return new UserResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "Không phải tài khoản Admin !",
                };
            }
          
            // 4. Check if login password match
            if (_encryptor.MD5Hash(req.Password) != admin.Password)
            {
                return new UserResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "Sai mật khẩu hoặc tên đăng nhập !",
                };
            }

            return new UserResponse
            {
                User = admin,
                IsSuccess = true
            };
        }
    }
}
