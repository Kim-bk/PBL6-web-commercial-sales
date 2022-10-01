using System;
using System.Threading.Tasks;
using ComercialClothes.Models.DTOs.Requests;
using CommercialClothes.Models;
using CommercialClothes.Models.DAL;
using CommercialClothes.Models.DAL.Repositories;
using CommercialClothes.Models.DTOs.Requests;
using CommercialClothes.Services.Base;

namespace CommercialClothes.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly Encryptor _encryptor;

        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork, Encryptor encryptor) : base(unitOfWork)
        {
            _userRepository = userRepository;
            _encryptor = encryptor;
        }
        public async Task<bool> Login(LoginRequest req)
        {
            // 1. Find user by user name
            var user = await _userRepository.FindAsync(us => us.UserName == req.UserName);

            // 2. Check if user exist
            if (user == null)
            {
                throw new ArgumentNullException("Can't find user!");
            }

            // 3. Check if login password match
            if (_encryptor.MD5Hash(req.PassWord) != user.Password)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> Register(RegistRequest req)
        {
            try
            {
                // 1. Check if duplicated username
             
                if (await _userRepository.FindAsync(us => us.UserName == req.UserName) != null)
                {
                    throw new Exception("UserName is already existed!");
                }

                // 2. Check pass with confirm pass
                if (!String.Equals(req.PassWord, req.ConfirmPassWord))
                {
                    throw new ArgumentException("Confirm Password not match!");
                }

                await _unitOfWork.BeginTransaction();

                // 3. Create new account
                var user = new Account
                {
                    UserName = req.UserName,
                    // 4. Encrypt password
                    Password = _encryptor.MD5Hash(req.PassWord),
                    DateCreated = DateTime.Now.Date
                };

                await _userRepository.AddAsync(user);
                await _unitOfWork.CommitTransaction();

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<bool> UpdateUser(UserRequest req)
        {
            try
            {
                var userReq = await _userRepository.FindAsync(it => it.Id == req.Id);
                if(userReq == null)
                {
                    throw new Exception("User not found!!");
                }
                await _unitOfWork.BeginTransaction();
                userReq.Name = req.Name;
                userReq.PhoneNumber = req.PhoneNumber;
                userReq.Address = req.Address;
                _userRepository.Update(userReq);
                await _unitOfWork.CommitTransaction();
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
            throw new NotImplementedException();
        }
    }
}
