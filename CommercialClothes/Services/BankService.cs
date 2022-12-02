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
    public class BankService : BaseService ,IBankService
    {
        private readonly IBankRepository _bankRepository;
        private readonly IUserRepository _userRepository;
        public BankService(IUnitOfWork unitOfWork, IMapperCustom mapper,IBankRepository bankRepository
                           ,IUserRepository userRepository) : base(unitOfWork, mapper)
        {
            _bankRepository = bankRepository;
            _userRepository = userRepository;
        }

        public async Task<BankResponse> AddBank(BankRequest req, int idAccount)
        {
            try
            {
                var user = await _userRepository.FindAsync(us => us.Id == idAccount);
                await _unitOfWork.BeginTransaction();
                var bank = new Bank()
                {
                    BankName = req.BankName,
                    BankNumber = req.BankNumber,
                    UserName = req.UserName,
                    AccountId = user.Id,
                    ExpiredDate = req.ExpiredDate,
                    StartedDate = req.StartedDate,
                };
                await _bankRepository.AddAsync(bank);   
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
            var user = await _userRepository.FindAsync(us => us.Id == idAccount);
            var banks = await _bankRepository.ListBank(user.Id);
            var bank = new List<BankDTO>();
            foreach (var item in banks)
            {
                var bankDTO = new BankDTO()
                {
                    Id = item.Id,
                    BankNumber = item.BankNumber,
                    BankName = item.BankName,
                    UserName = item.UserName,
                    ExpiredDate = item.ExpiredDate,
                    StartedDate = item.StartedDate
                };
                bank.Add(bankDTO);
            }
            return bank;
        }

        public async Task<BankResponse> UpdateBank(BankRequest req, int idAccount)
        {
            try
            {
                var user = await _userRepository.FindAsync(us => us.Id == idAccount);
                var findBank = await _bankRepository.FindAsync(bk => bk.Id == req.Id);
                await _unitOfWork.BeginTransaction();
                findBank.BankName = req.BankName;
                findBank.BankNumber = req.BankNumber;
                findBank.UserName = req.UserName;
                findBank.AccountId = user.Id;
                findBank.ExpiredDate = req.ExpiredDate;
                findBank.StartedDate = req.StartedDate;
                _bankRepository.Update(findBank);   
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