using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebNC_Project.Models;

namespace WebNC_Project.Areas.Server
{
    public class ServerAuthorize: AuthorizeAttribute
    {
        private readonly string[] allowedroles;
        public ServerAuthorize(params string[] roles)
        {
            this.allowedroles = roles;
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;
            var uid = Convert.ToString(httpContext.Session["UID"]);
            var role = Convert.ToString(httpContext.Session["Role"]);
            if (!string.IsNullOrEmpty(uid))
            {
                if (role == "MANAGER") return true;
                foreach (var permission in allowedroles)
                {
                    if (permission == role) return true;
                }

            }
            return authorize;
                /*using (var context = new ResortContext())
                {
                    var user = context.Staffs.Find(uid);
                    if (user.PermissionID == "MANAGER") return true;
                    foreach (var role in allowedroles)
                    {
                        if (role == user.PermissionID) return true;
                    }
                }*/
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
               new RouteValueDictionary
               {
                    { "controller", "Home" },
                    { "action", "UnAuthorized" }
               });
        }
    }
}