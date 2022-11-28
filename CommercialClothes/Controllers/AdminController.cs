using CommercialClothes.Commons.CustomAttribute;
using CommercialClothes.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CommercialClothes.Controllers
{
   // [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

     //   [Permission("MANAGE_PERMISSION")]
        [HttpGet("credentials/{userGroupId:int}")]
        public async Task<IActionResult> GetRolesOfUserGroup(int userGroupId)
        {
            var rs = await _adminService.GetRolesOfUserGroup(userGroupId);
            return Ok(rs);
        }
    }
}
