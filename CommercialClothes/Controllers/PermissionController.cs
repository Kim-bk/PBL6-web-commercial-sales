using System.Threading.Tasks;
using CommercialClothes.Models;
using CommercialClothes.Models.DTOs.Requests;
using CommercialClothes.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ComercialClothes.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    //[EnableCors("MyPolicy")]
    public class PermissionController : Controller
    {
        private readonly IPermissionService _permissionService;
        public PermissionController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [HttpPost("role")]
        // api/permission/role
        public async Task<IActionResult> CreateRole(string roleName)
        {
            var rs = await _permissionService.CreateRole(roleName);
            if (!rs.IsSuccess)
            {
                return BadRequest(rs.ErrorMessage);
            }    

            return Ok("Creata role " + roleName + "success !");
        }

        [HttpPut("role")]
        // api/permission/role
        public async Task<IActionResult> UpdateRole(RoleRequest req)
        {
            var rs = await _permissionService.UpdateRole(req);
            if (!rs.IsSuccess)
            {
                return BadRequest(rs.ErrorMessage);
            }

            return Ok("Update success !");
        }

        [HttpDelete("role/{roleId:int}")]
        // api/permission/role/{roleId}
        public async Task<IActionResult> DeleteRole(int roleId)
        {
            var rs = await _permissionService.DeleteRole(roleId);
            if (!rs.IsSuccess)
            {
                return BadRequest(rs.ErrorMessage);
            }

            return Ok("Delete success !");
        }

        [HttpPost("user-group")]
        // api/permission/user-group
        public async Task<IActionResult> AddUserGroup(string userGroupName)
        {
            var rs = await _permissionService.AddUserGroup(userGroupName);
            if (!rs.IsSuccess)
            {
                return BadRequest(rs.ErrorMessage);
            }

            return Ok("Add User Group " + userGroupName + " success !");
        }

        [HttpDelete("user-group/{userGroupId:int}")]
        // api/permission/user-group
        public async Task<IActionResult> DeleteUserGroup(int userGroupId)
        {
            var rs = await _permissionService.DeleteUserGroup(userGroupId);
            if (!rs.IsSuccess)
            {
                return BadRequest(rs.ErrorMessage);
            }
            return Ok("Delete User Group " + userGroupId.ToString());
        }

        [HttpPut("user-group")]
        // api/permission/user-group
        public async Task<IActionResult> UpdateUserGroup(UserGroupRequest request)
        {
            var rs = await _permissionService.UpdateUserGroup(request);
            if (!rs.IsSuccess)
            {
                return BadRequest(rs.ErrorMessage);
            }

            return Ok();
        }

        [HttpPost("credential")]
        // api/permission/credential
        public async Task<IActionResult> AddCredential(CredentialRequest req)
        {
            var rs = await _permissionService.AddCredential(req);
            if (!rs.IsSuccess)
            {
                return BadRequest(rs.ErrorMessage);
            }

            return Ok();
        }

        [HttpDelete("credential")]
        // api/permission/credential
        public async Task<IActionResult> RemoveCredential(CredentialRequest req)
        {
            var rs = await _permissionService.RemoveCredential(req);
            if (!rs.IsSuccess)
            {
                return BadRequest(rs.ErrorMessage);
            }

            return Ok();
        }
    }
}
