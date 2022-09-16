using System;
using System.Threading.Tasks;
using ComercialClothes.Models;
using ComercialClothes.Models.DAL;
using ComercialClothes.Models.DAL.Repositories;
using ComercialClothes.Models.DTOs.Requests;
using CommercialClothes.Services;
using CommercialClothes.Services.Base;

namespace ComercialClothes.Services
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
    }
}
