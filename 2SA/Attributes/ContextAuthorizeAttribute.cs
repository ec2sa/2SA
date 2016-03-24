using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eArchiver.Models;
using System.Web.Security;
using eArchiver.Controllers;

namespace eArchiver.Attributes
{
    public class ContextAuthorizeAttribute:AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (AuthorizeCore(filterContext.HttpContext))
            {
                if (string.IsNullOrEmpty(this.Roles))
                    return;

                if (AppContext.IsUserInRoleInContext(this.Roles))
                {
                    return;
                }
                else
                {
                    filterContext.Result = new HttpUnauthorizedResult();
                }
            }
            filterContext.Result = new HttpUnauthorizedResult();
        }
    }
}
