using FactFluxV3.Areas.Identity.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
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
            var user = context.HttpContext.User.Identities;

            var userName = user.FirstOrDefault().Name;

            if(userName == "alex")
            {
                return;
            }
            else
            {
                throw new Exception("NO");
            }
        }
    }
}
