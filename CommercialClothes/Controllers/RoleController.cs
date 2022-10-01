using System.Threading.Tasks;
using CommercialClothes.Models.DTOs.Requests;
using CommercialClothes.Services.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ComercialClothes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[EnableCors("MyPolicy")]
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost]
        // api/role
        public async Task<IActionResult> CreateRole(string roleName)
        {
            await _roleService.CreateRole(roleName);
            return Ok("Creata role " + roleName + "success !");
        }

        [HttpPut]
        // api/role
        public async Task<IActionResult> CreateRole(RoleRequest req)
        {
            await _roleService.UpdateRole(req);
            return Ok("Update success !");
        }

        [HttpDelete]
        // api/role
        public async Task<IActionResult> DeleteRole(int roleId)
        {
            await _roleService.DeleteRole(roleId);
            return Ok("Delete success !");
        }

        [HttpPost("user-group")]
        // api/role/user-group
        public async Task<IActionResult> AddUserGroup(string userGroupName)
        {
            await _roleService.AddUserGroup(userGroupName);
            return Ok();
        }

        [HttpDelete("user-group")]
        // api/role/user-group
        public async Task<IActionResult> DeleteUserGroup(int userGroupId)
        {
            await _roleService.DeleteUserGroup(userGroupId);
            return Ok();
        }

        [HttpPut("credential")]
        // api/role/credential
        public async Task<IActionResult> AddCredential(CredentialRequest req)
        {
            return Ok();
        }
    }
}
