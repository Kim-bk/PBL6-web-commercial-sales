using Castle.Core.Resource;
using ComercialClothes.Models.DTOs.Requests;
using CommercialClothes.Models.DAL;
using CommercialClothes.Models.DAL.Interfaces;
using CommercialClothes.Models.DAL.Repositories;
using CommercialClothes.Models.DTOs;
using CommercialClothes.Models.DTOs.Responses;
using CommercialClothes.Services.Base;
using CommercialClothes.Services.Interfaces;
using Model.DAL.Interfaces;
using Model.DTOs;
using Model.DTOs.Requests;
using Model.DTOs.Responses;
using Model.Entities;
using Org.BouncyCastle.Ocsp;
using System;
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
        private readonly IHistoryTransactionRepository _historyTransactionRepo;
        private readonly IShopRepository _shopRepo;
        private readonly Encryptor _encryptor;

        public AdminService(ICredentialRepository credentialRepo, IMapperCustom mapper
            , IUnitOfWork unitOfWork, IRoleRepository roleRepo
            , IUserRepository userRepo, Encryptor encryptor
            , IHistoryTransactionRepository historyTransactionRepo
            , IShopRepository shopRepo) : base(unitOfWork, mapper)
        {
            _credentialRepo = credentialRepo;
            _roleRepo = roleRepo;
            _userRepo = userRepo;
            _historyTransactionRepo = historyTransactionRepo;
            _encryptor = encryptor;
            _shopRepo = shopRepo;
        }

        public async Task<bool> ManageTransaction(TransactionDTO transactionDto, int userGroupId)
        {
            // Find account admin
            var admin = await _userRepo.FindAsync(us => us.UserGroupId == 1);
            if (userGroupId == 1)
            {
                // Add money to account admin
                admin.Wallet += transactionDto.Money;

                // Save to history transaction that order is prepared
                var history = new HistoryTransaction
                {
                    BillId = transactionDto.BillId,
                    CustomerId = transactionDto.CustomerId,
                    ShopId = transactionDto.ShopId,
                    Money = transactionDto.Money,
                    TransactionDate = DateTime.Now,
                    StatusId = 1,
                };
                await _historyTransactionRepo.AddAsync(history);
            }

            if (userGroupId == 2)
            {
                // Find account customer
                var customer = await _userRepo.FindAsync(us => us.Id == transactionDto.CustomerId);
                customer.Wallet += transactionDto.Money;
                admin.Wallet -= transactionDto.Money;

                // Save to history transaction that order canceled
                var history = new HistoryTransaction
                {
                    BillId = transactionDto.BillId,
                    CustomerId = transactionDto.CustomerId,
                    ShopId = transactionDto.ShopId,
                    Money = transactionDto.Money,
                    TransactionDate = DateTime.Now,
                    StatusId = 4,
                };
                await _historyTransactionRepo.AddAsync(history);
            }

            if (userGroupId == 3)
            {
                // Find shop account and wallet
                var shop = await _userRepo.FindAsync(us => us.Id == transactionDto.CustomerId);
                shop.Shop.ShopWallet += transactionDto.Money;
                admin.Wallet -= transactionDto.Money;

                // Save to history transaction that order completed
                var history = new HistoryTransaction
                {
                    BillId = transactionDto.BillId,
                    CustomerId = transactionDto.CustomerId,
                    ShopId = transactionDto.ShopId,
                    Money = transactionDto.Money,
                    TransactionDate = DateTime.Now,
                    StatusId = 3,
                };
                await _historyTransactionRepo.AddAsync(history);
            }

            await _unitOfWork.CommitTransaction();
            return true;
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

        public async Task<List<UserDTO>> GetUsers()
        {
            var rs = await _userRepo.GetAccounts();
            return _mapper.MapUsers(rs);
        }

        public async Task<UserResponse> Login(LoginRequest req)
        {
            // 1. Find admin account
            var admin = await _userRepo.FindAsync(us => us.Email == req.Email && (us.UserGroupId == 1 || us.UserGroupId == 4));

            // 2. Check if user exist
            if (admin == null)
            {
                return new UserResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "Không phải tài khoản Admin !",
                };
            }

            // 3. Check if login password match
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

        public async Task<bool> UpdateUserGroupOfUser(UserGroupUpdatedRequest request)
        {
            var user = await _userRepo.FindAsync(u => u.Id == request.UserId);
            user.UserGroupId = request.UserGroupId;
            await _unitOfWork.CommitTransaction();
            return true;
        }

        public async Task<List<TransactionResponse>> GetTransactions()
        {
            var result = new List<TransactionResponse>();
            var allTransactions = await _historyTransactionRepo.GetAll();
            foreach (var transaction in allTransactions)
            {
                var transactionRes = new TransactionResponse
                {
                    BillId = transaction.BillId,
                    ShopName = (await _shopRepo.FindAsync(s => s.Id == transaction.ShopId)).Name,
                    CustomerName = (await _userRepo.FindAsync(us => us.Id == transaction.CustomerId)).Name,
                    TransactionDate = transaction.TransactionDate,
                    Status = transaction.Status.Name,
                };

                if (transaction.StatusId == 1)
                {
                    transactionRes.Money = "+" + transaction.Money.ToString();
                }

                if (transaction.StatusId == 3)
                {
                    transactionRes.Money = "-" + transaction.Money.ToString();
                }

                if (transaction.StatusId == 4)
                {
                    transactionRes.Money = "-" + transaction.Money.ToString();
                }

                result.Add(transactionRes);
            }
            return result.OrderByDescending(rs => rs.TransactionDate).ToList();
        }
    }
}