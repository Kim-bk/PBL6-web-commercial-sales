using CommercialClothes.Models.DAL;
using CommercialClothes.Models.DAL.Interfaces;
using CommercialClothes.Models.DTOs.Requests;
using CommercialClothes.Models.DTOs.Responses.Base;
using CommercialClothes.Services.Base;
using System.Threading.Tasks;
using System;
using CommercialClothes.Models;
using CommercialClothes.Models.DTOs.Responses;

namespace CommercialClothes.Services.Interfaces
{
    public class UserGroupService : BaseService, IUserGroupService
    {
        private readonly IUserGroupRepository _userGroupRepository;
        public UserGroupService(IUserGroupRepository userGroupRepository, IUnitOfWork unitOfWork
                    , IMapperCustom mapper) : base(unitOfWork, mapper)
        {
            _userGroupRepository = userGroupRepository;
        }

        public async Task<GeneralResponse> UpdateUserGroup(UserGroupRequest req)
        {
            try
            {
                // 1. Find User Group
                var userGroup = await _userGroupRepository.FindAsync(ug => ug.Id == req.UserGroupId);
                if (userGroup == null)
                {
                    return new GeneralResponse
                    {
                        IsSuccess = false,
                        ErrorMessage = "Không tìm thấy tên quyền " + req.Name + " !"
                    };
                }

                // 2. Update that User Group
                userGroup.Name = req.Name;
                _userGroupRepository.Update(userGroup);
                await _unitOfWork.CommitTransaction();
                return new GeneralResponse
                {
                    IsSuccess = true
                };


            }
            catch (Exception e)
            {
                return new GeneralResponse
                {
                    IsSuccess = false,
                    ErrorMessage = e.Message
                };
            }
        }
        public async Task<GeneralResponse> AddUserGroup(string userGroupName)
        {
            try
            {
                // 1. Validate
                if (String.IsNullOrEmpty(userGroupName))
                {
                    return new GeneralResponse
                    {
                        IsSuccess = false,
                        ErrorMessage = "Cú pháp không hợp lệ !"
                    };
                }

                var userGroup = await _userGroupRepository.FindAsync(ug => ug.Name == userGroupName);
                if (userGroup != null)
                {
                    return new GeneralResponse
                    {
                        IsSuccess = false,
                        ErrorMessage = "User Group đã tồn tại !"
                    };
                }

                var newUserGroup = new UserGroup
                {
                    Name = userGroupName
                };
                await _userGroupRepository.AddAsync(newUserGroup);
                await _unitOfWork.CommitTransaction();
                return new GeneralResponse
                {
                    IsSuccess = true,
                };
            }
            catch (Exception e)
            {
                return new GeneralResponse
                {
                    IsSuccess = false,
                    ErrorMessage = e.Message,
                };
            }
        }

        public async Task<GeneralResponse> DeleteUserGroup(int userGroupId)
        {
            try
            {
                await _userGroupRepository.Delete(ug => ug.Id == userGroupId);
                await _unitOfWork.CommitTransaction();
                return new GeneralResponse
                {
                    IsSuccess = true,
                };
            }

            catch (Exception e)
            {
                return new GeneralResponse
                {
                    IsSuccess = false,
                    ErrorMessage = e.Message
                };
            }
        }

        public async Task<UserGroupResponse> GetUserGroups()
        {
            try
            {
                var rs = await _userGroupRepository.GetAll();
                return new UserGroupResponse
                { 
                    IsSuccess = true,   
                    UserGroups = rs
                };

            }
            catch (Exception e)
            {
                return new UserGroupResponse
                {
                    IsSuccess = false,
                    ErrorMessage = e.Message
                };
            }
        }
    }
}
