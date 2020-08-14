using Forum.Controllers.Auntification;
using Forum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Forum.Filters
{
    public class AuthorizeRoleAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            string[] allowedRoles = new string[] { };
            if (!String.IsNullOrEmpty(base.Roles))
            {
                allowedRoles = base.Roles.Split(new char[] { ',' });
                for (int i = 0; i < allowedRoles.Length; i++)
                {
                    allowedRoles[i] = allowedRoles[i].Trim();
                }
            }
            bool IsAut = allowedRoles.Contains(Authentification._role);
            if (!IsAut)
            {
                AuthorizationContext filterContext = new AuthorizationContext();
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary {
                { "controller", "Account" }, { "action", "Login" }
                });
            }
            return true;
        }
    }
}