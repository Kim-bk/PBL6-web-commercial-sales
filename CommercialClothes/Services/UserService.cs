using System;
using System.Threading.Tasks;
using ComercialClothes.Models;
using ComercialClothes.Models.DAL;
using ComercialClothes.Models.DAL.Repositories;
using ComercialClothes.Models.DTOs.Requests;
using CommercialClothes.Services;
using CommercialClothes.Services.Base;

namespace CommercialClothes.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly Encryptor _encryptor;
        private readonly IEmailSender _emailSender;

        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork, Encryptor encryptor
                    , IEmailSender emailSender) : base(unitOfWork)
        {
            _userRepository = userRepository;
            _encryptor = encryptor;
            _emailSender = emailSender;
        }

        public async Task<bool> CheckUserByActivationCode(Guid activationCode)
        {
            var user = await _userRepository.FindAsync(us => us.ActivationCode == activationCode);
            if (user == null)
                return false;

            user.IsActivated = true;
            await _unitOfWork.CommitTransaction();
            return true;
        }

        public async Task<bool> Login(LoginRequest req)
        {
            // 1. Find user by user name
            var user = await _userRepository.FindAsync(us => us.Email == req.Email);

            // 2. Check if user exist
            if (user == null)
            {
                throw new ArgumentException("Can't find user!");
            }

            // 3. Check if user is activated
            if (!user.IsActivated)
            {
                throw new Exception("Please check your Email to activate!");
            }

            // 4. Check if login password match
            if (_encryptor.MD5Hash(req.PassWord) != user.Password)
            {
                throw new ArgumentException("Wrong Email or Password!");
            }
            return true;
        }

        public async Task<bool> Register(RegistRequest req)
        {
            try
            {
                // 1. Check if duplicated account created
                var getUser = await _userRepository.FindAsync(us => us.Email == req.Email && us.IsActivated == true);
                if (getUser != null)
                {
                    throw new Exception("Email is already used!");
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
                    Email = req.Email,
                    IsActivated = false,
                    ActivationCode = Guid.NewGuid(),

                    // 4. Encrypt password
                    Password = _encryptor.MD5Hash(req.PassWord),
                    DateCreated = DateTime.Now.Date
                };

                await _userRepository.AddAsync(user);

                // 4. Add user 
                await _unitOfWork.CommitTransaction();

                // 4. Send an email activation
                await _emailSender.SendEmailVerificationAsync(user.Email, user.ActivationCode.ToString(), "verify-account");

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
