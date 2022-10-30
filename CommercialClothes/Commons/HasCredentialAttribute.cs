using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace WebBookStore.Common
{
    public class HasCredentialAttribute : AuthorizeAttribute
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public HasCredentialAttribute(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string RoleName { get; set; }
        public bool AuthorizeCore()
        {
            string credentials = _httpContextAccessor.HttpContext.User.FindFirstValue("Credentials");
            if (credentials == null)
                return false;
            else
            {
                if (credentials.Contains(RoleName))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}