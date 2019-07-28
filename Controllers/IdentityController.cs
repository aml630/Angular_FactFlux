using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FactFluxV3.Areas.Identity.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FactFluxV3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IHttpContextAccessor HttpContext;
        private readonly UserManager<IdentityUser> UserManager;
        private readonly FactFluxIdentity FactFluxIdentityContext;

        public IdentityController(IHttpContextAccessor httpContext, UserManager<IdentityUser> userManager, FactFluxIdentity factFluxIdentity)
        {
            HttpContext = httpContext;
            UserManager = userManager;
            FactFluxIdentityContext = factFluxIdentity;
        }

        [HttpGet]
        public async Task<IdentityUser> Get()
        {
            var user = await UserManager.GetUserAsync(HttpContext.HttpContext.User);

            return user;
        }

        [HttpGet("IsAdmin")]
        public async Task<bool> IsAdmin()
        {
            bool isAdmin = false;
            var user = await UserManager.GetUserAsync(HttpContext.HttpContext.User);

            if (user == null)
            {
                return isAdmin;
            }

            var userRoles = FactFluxIdentityContext.UserRoles.Where(x => x.UserId == user.Id).Select(x => x.RoleId).ToList();

            if (userRoles.Contains("1"))
            {
                isAdmin = true;
            }

            return isAdmin;
        }
    }
}