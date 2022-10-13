using System;
using System.Diagnostics;
using System.Threading.Tasks;
using ComercialClothes.Models.DTOs.Requests;
using CommercialClothes.Models;
using CommercialClothes.Models.DAL;
using CommercialClothes.Models.DAL.Interfaces;
using CommercialClothes.Models.DAL.Repositories;
using CommercialClothes.Models.DTOs.Requests;
using CommercialClothes.Models.DTOs.Responses;
using CommercialClothes.Services.Base;
using CommercialClothes.Services.Interfaces;

namespace CommercialClothes.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly Encryptor _encryptor;
        private readonly IEmailSender _emailSender;

        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork, Encryptor encryptor
                    , IEmailSender emailSender, IMapperCustom mapper
                    , IRefreshTokenRepository refreshTokenRepossitory) : base(unitOfWork, mapper)
        {
            _userRepository = userRepository;
            _encryptor = encryptor;
            _emailSender = emailSender;
            _refreshTokenRepository = refreshTokenRepossitory;

        }

        public async Task<Account> FindById(int userId)
        {
            return await _userRepository.FindAsync(us => us.Id == userId);
        }
        public async Task<bool> Logout(int userId)
        {
            try
            {
                await _refreshTokenRepository.DeleteAll(userId);
                await _unitOfWork.CommitTransaction();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
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

        public async Task<UserResponse> ForgotPassword(string userEmail)
        {
            UserResponse res = new UserResponse();
            try
            {
                // 1. Find user by email
                var user = await _userRepository.FindAsync(us => us.Email == userEmail);
                
                // 2. Check
                if (user == null)
                {
                    return new UserResponse
                    {
                        IsSuccess = false,
                        ErrorMesage = "Không thể tìm thấy Email được đăng ký !",
                    };
                }

                // 3. Generate reset pass word code to authenticate
                var resetCode = Guid.NewGuid();
                user.ResetPasswordCode = resetCode;

                // 3. Send email to user to reset password
                await _emailSender.SendEmailVerificationAsync(userEmail, resetCode.ToString(), "reset-password");

                await _unitOfWork.CommitTransaction();

                return new UserResponse
                {
                    IsSuccess = true
                };
            }

            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> GetUserByResetCode(Guid resetPassCode)
        {
            return await _userRepository.FindAsync(us => us.ResetPasswordCode == resetPassCode) != null;
        }

        public async Task<UserResponse> Login(LoginRequest req)
        {
            // 1. Find user by user name
            var user = await _userRepository.FindAsync(us => us.Email == req.Email);

            // 2. Check if user exist
            if (user == null)
            {
                return new UserResponse
                {
                    IsSuccess = false,
                    ErrorMesage = "Không thể tìm thấy tài khoản !",
                };
            }

            // 3. Check if user is activated
            if (!user.IsActivated)
            {
                return new UserResponse
                {
                    IsSuccess = false,
                    ErrorMesage = "Vui lòng kiểm tra Email để kích hoạt tài khoản !",
                };
            }

            // 4. Check if login password match
            if (_encryptor.MD5Hash(req.Password) != user.Password)
            {
                return new UserResponse
                {
                    IsSuccess = false,
                    ErrorMesage = "Sai mật khẩu hoặc tên đăng nhập !",
                };
            }

            return new UserResponse
            {
                User = user,
                IsSuccess = true
            };
        }

        public async Task<UserResponse> Register(RegistRequest req)
        {
            try
            {
                // 1. Check if duplicated account created
                var getUser = await _userRepository.FindAsync(us => us.Email == req.Email && us.IsActivated == true);
              
                if (getUser != null)
                {
                    return new UserResponse
                    {
                        IsSuccess = false,
                        ErrorMesage = "Email đã được sử dụng !",
                    };
                }

                // 2. Check pass with confirm pass
                if (!String.Equals(req.Password, req.ConfirmPassWord))
                {
                    return new UserResponse
                    {
                        IsSuccess = false,
                        ErrorMesage = "Mật khẩu xác nhận không khớp !",
                    };
                }

                await _unitOfWork.BeginTransaction();

                // 3. Create new account
                var user = new Account
                {
                    Email = req.Email,
                    IsActivated = false,
                    ActivationCode = Guid.NewGuid(),

                    // 4. Encrypt password
                    Password = _encryptor.MD5Hash(req.Password),
                    DateCreated = DateTime.UtcNow.Date
                };

                await _userRepository.AddAsync(user);

                // 4. Add user 
                await _unitOfWork.CommitTransaction();

                // 4. Send an email activation
                await _emailSender.SendEmailVerificationAsync(user.Email, user.ActivationCode.ToString(), "verify-account");

                return new UserResponse
                {
                    IsSuccess = true,
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserResponse> ResetPassword(ResetPasswordRequest request)
        {
            try
            {
                // 1. Find user by reset password code
                var user = await _userRepository.FindAsync(us => us.ResetPasswordCode == request.ResetPasswordCode);

                // 2. Check
                if (user == null)
                {
                    return new UserResponse
                    {
                        IsSuccess = false,
                        ErrorMesage = "Không tìm thấy tài khoản !",
                    };
                }

                user.Password = request.NewPassword;
                user.ResetPasswordCode = new Guid();

                await _unitOfWork.CommitTransaction();

                return new UserResponse
                {
                    IsSuccess = true,
                };
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<UserResponse> UpdateUser(UserRequest req)
        {
            try
            {
                var userReq = await _userRepository.FindAsync(it => it.Id == req.Id);
                if(userReq == null)
                {
                    return new UserResponse
                    {
                        IsSuccess = false,
                        ErrorMesage = "Không tìm thấy tài khoản !",
                    };
                }
                await _unitOfWork.BeginTransaction();
                userReq.Name = req.Name;
                userReq.PhoneNumber = req.PhoneNumber;
                userReq.Address = req.Address;
                _userRepository.Update(userReq);
                await _unitOfWork.CommitTransaction();
                return new UserResponse
                {
                    IsSuccess = true,
                };
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
