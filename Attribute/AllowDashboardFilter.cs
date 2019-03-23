using FactFlux;
using FactFluxV3.Areas.Identity.Data;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactFluxV3.Attribute
{
    public class AllowAllDashboardAuthorizationFilter : IDashboardAuthorizationFilter
    {
        private readonly IHttpContextAccessor HttpContext;

        public AllowAllDashboardAuthorizationFilter(IHttpContextAccessor httpContext)
        {
            HttpContext = httpContext;
        }

        public bool Authorize(DashboardContext context)
        {
            var optionsBuilder = new DbContextOptionsBuilder<FactFluxIdentity>();
            optionsBuilder.UseSqlServer(Startup.staticConfig["ConnectionStrings:FactFluxConnection"]);
            var newdbContext = new FactFluxIdentity(optionsBuilder.Options);

            var userClaims = HttpContext.HttpContext.User.Identities.FirstOrDefault().Claims;

            if (!userClaims.Any())
            {
                return false;
            }

            var userId = userClaims.FirstOrDefault().Value.ToString();

            using (newdbContext)
            {
                var foundUser = newdbContext.Users.Where(x => x.Id == userId).FirstOrDefault();

                if (foundUser == null)
                {
                    return false;
                }

                var isAdmin = (from ur in newdbContext.UserRoles
                               join r in newdbContext.Roles on ur.RoleId equals r.Id
                               where r.Name == "Admin" && ur.UserId == userId
                               select ur).FirstOrDefault();

                if (isAdmin == null)
                {
                    return false;
                }

                return true;
            }
        }
    }
}
