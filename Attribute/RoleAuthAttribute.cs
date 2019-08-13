using FactFlux;
using FactFluxV3.Areas.Identity.Data;
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
    public class RoleAuthAttribute : ActionFilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var optionsBuilder = new DbContextOptionsBuilder<FactFluxIdentity>();
            optionsBuilder.UseSqlServer(Startup.staticConfig["ConnectionStrings:FactFluxConnection"]);
            var newdbContext = new FactFluxIdentity(optionsBuilder.Options);
            var userClaims = context.HttpContext.User.Identities.FirstOrDefault().Claims;

            if (userClaims == null || userClaims.Count() == 0)
            {
                context.Result = new ForbidResult();
            }

            var userId = userClaims.FirstOrDefault().Value.ToString();

            using (newdbContext)
            {
                var foundUser = newdbContext.Users.Where(x => x.Id == userId).FirstOrDefault();

                if (foundUser == null)
                {
                    context.Result = new ForbidResult();
                }

                var isAdmin = (from ur in newdbContext.UserRoles
                               join r in newdbContext.Roles on ur.RoleId equals r.Id
                               where r.Name == "Admin" && ur.UserId == userId
                               select ur).FirstOrDefault();

                if (isAdmin == null)
                {
                    context.Result = new ForbidResult();
                }
            }
        }
    }
}
