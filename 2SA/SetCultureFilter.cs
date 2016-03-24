using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;

namespace eArchiver
{
    public class SetCultureFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string lang;
            lang = filterContext.HttpContext.Session["language"] == null ? string.Empty : filterContext.HttpContext.Session["language"].ToString();

            if (string.IsNullOrEmpty(lang))
            {
                if (filterContext.HttpContext.Request.UserLanguages!=null&& filterContext.HttpContext.Request.UserLanguages.Length > 0)
                    lang = filterContext.HttpContext.Request.UserLanguages[0];
            }
            if (string.IsNullOrEmpty(lang))
                lang = "en-US";

            CultureInfo ci = new CultureInfo(lang);
            System.Threading.Thread.CurrentThread.CurrentCulture = ci;
            System.Threading.Thread.CurrentThread.CurrentUICulture = ci;
        }
    }
}