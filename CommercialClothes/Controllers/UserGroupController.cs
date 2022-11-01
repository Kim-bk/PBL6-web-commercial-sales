using CommercialClothes.Commons.CustomAttribute;
using CommercialClothes.Models.DTOs.Requests;
using CommercialClothes.Services;
using CommercialClothes.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CommercialClothes.Controllers
{
    [Authorize]
    [Permission("MANAGE_PERMISSION")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserGroupController : Controller
    {
        private readonly IUserGroupService _userGroupService;
        public UserGroupController(IUserGroupService userGroupService)
        {
            _userGroupService = userGroupService;
        }

        [HttpGet]
        // api/usergroup
        public async Task<IActionResult> GetUserGroups()
        {
            var rs = await _userGroupService.GetUserGroups();
            if (!rs.IsSuccess)
            {
                return BadRequest(rs.ErrorMessage);
            }
            return Ok(rs.UserGroups);
        }

        [HttpPost]
        // api/usergroup
        public async Task<IActionResult> AddUserGroup([FromBody] string userGroupName)
        {
            var rs = await _userGroupService.AddUserGroup(userGroupName);
            if (!rs.IsSuccess)
            {
                return BadRequest(rs.ErrorMessage);
            }

            return Ok("Add User Group " + userGroupName + " success !");
        }

        [HttpDelete("{userGroupId:int}")]
        // api/usergroup/userGroupId
        public async Task<IActionResult> DeleteUserGroup(int userGroupId)
        {
            var rs = await _userGroupService.DeleteUserGroup(userGroupId);
            if (!rs.IsSuccess)
            {
                return BadRequest(rs.ErrorMessage);
            }
            return Ok("Delete User Group " + userGroupId.ToString());
        }

        [HttpPut]
        // api/usergroup
        public async Task<IActionResult> UpdateUserGroup(UserGroupRequest request)
        {
            var rs = await _userGroupService.UpdateUserGroup(request);
            if (!rs.IsSuccess)
            {
                return BadRequest(rs.ErrorMessage);
            }

            return Ok();
        }
    }
}
