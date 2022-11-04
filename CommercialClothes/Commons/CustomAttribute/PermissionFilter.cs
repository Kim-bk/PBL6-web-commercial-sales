using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace CommercialClothes.Commons.CustomAttribute
{
    public class PermissionFilter : IAuthorizationFilter
    {
        private readonly string _role;

        public PermissionFilter(string role)
        {
            _role = role;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // 1. Get all credentials of the user
            var userCredentials = context.HttpContext.User.FindFirst("Credentials")?.Value;

            // 2. Check user credential has role
            var hasClaim = userCredentials.Contains(_role);
            if (!hasClaim)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}