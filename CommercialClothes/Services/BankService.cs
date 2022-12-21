using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommercialClothes.Models.DAL;
using CommercialClothes.Models.DAL.Interfaces;
using CommercialClothes.Models.DAL.Repositories;
using CommercialClothes.Models.DTOs;
using CommercialClothes.Models.DTOs.Requests;
using CommercialClothes.Models.DTOs.Responses;
using CommercialClothes.Models.Entities;
using CommercialClothes.Services.Base;
using CommercialClothes.Services.Interfaces;

namespace CommercialClothes.Services
{
    public class BankService : BaseService, IBankService
    {
        private readonly IBankRepository _bankRepo;
        private readonly IUserRepository _userRepo;
        private readonly IBankTypeRepository _bankTypeRepo;

        public BankService(IUnitOfWork unitOfWork, IMapperCustom mapper, IBankRepository bankRepository
                           , IUserRepository userRepo, IBankTypeRepository bankTypeRepository) : base(unitOfWork, mapper)
        {
            _bankRepo = bankRepository;
            _userRepo = userRepo;
            _bankTypeRepo = bankTypeRepository;
        }

        public async Task<BankResponse> AddBank(BankRequest req, int idAccount)
        {
            try
            {
                var user = await _userRepo.FindAsync(us => us.Id == idAccount);
                await _unitOfWork.BeginTransaction();
                var bank = new Bank()
                {
                    BankNumber = req.BankNumber,
                    AccountName = req.AccountName,
                    AccountId = user.Id,
                    BankTypeId = req.BankTypeId,
                    ExpiredDate = req.ExpiredDate,
                    StartedDate = req.StartedDate,
                };
                await _bankRepo.AddAsync(bank);
                await _unitOfWork.CommitTransaction();
                return new BankResponse()
                {
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new BankResponse()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message,
                };
            }
        }

        public async Task<List<BankDTO>> GetBankById(int idAccount)
        {
            var user = await _userRepo.FindAsync(us => us.Id == idAccount);
            var banks = await _bankRepo.GetUserBanks(user.Id);
            var bank = new List<BankDTO>();
            foreach (var item in banks)
            {
                var bankDTO = new BankDTO()
                {
                    Id = item.Id,
                    BankNumber = item.BankNumber,
                    AccountName = item.AccountName,

                    ExpiredDate = item.ExpiredDate,
                    StartedDate = item.StartedDate
                };
                bank.Add(bankDTO);
            }
            return bank;
        }

        public async Task<BankResponse> GetBanksByUser(int userId)
        {
            try
            {
                var user = await _userRepo.FindAsync(us => us.Id == userId);
                var userBanks = await _bankRepo.GetUserBanks(user.Id);
                var listBanks = new List<BankDTO>();
                foreach (var bank in userBanks)
                {
                    var bankDTO = new BankDTO()
                    {
                        Id = bank.Id,
                        BankNumber = bank.BankNumber,
                        BankName = bank.BankType.BankName,
                        BankCode = bank.BankType.BankCode,
                        AccountName = bank.AccountName,
                        ExpiredDate = bank.ExpiredDate,
                        StartedDate = bank.StartedDate
                    };
                    listBanks.Add(bankDTO);
                }
                return new BankResponse
                {
                    IsSuccess = true,
                    UserBanks = listBanks,
                };
            }
            catch (Exception e)
            {
                return new BankResponse
                {
                    IsSuccess = false,
                    ErrorMessage = e.ToString()
                };
            }
        }

        public Task<List<BankType>> GetBankType()
        {
            return _bankTypeRepo.GetAllBanksType();
        }

        public async Task<BankResponse> UpdateBank(BankRequest req, int idAccount)
        {
            try
            {
                var user = await _userRepo.FindAsync(us => us.Id == idAccount);
                var findBank = await _bankRepo.FindAsync(bk => bk.Id == req.Id);
                findBank.BankNumber = req.BankNumber;
                findBank.AccountName = req.AccountName;
                findBank.AccountId = user.Id;
                findBank.BankTypeId = req.BankTypeId;
                findBank.ExpiredDate = req.ExpiredDate;
                findBank.StartedDate = req.StartedDate;
                _bankRepo.Update(findBank);
                await _unitOfWork.CommitTransaction();
                return new BankResponse()
                {
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new BankResponse()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message,
                };
            }
        }
    }
}