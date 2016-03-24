using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using eArchiver.Models;
using eArchiver;

namespace eArchiver.Controllers
{
    [HandleError]
    [JsonRequestBehavior]
    public class BaseController : Controller
    {
        protected EArchiverDataContext Context = new EArchiverDataContext();

        protected override void OnException(ExceptionContext filterContext)
        {
            //WriteLog(Settings.LogErrorFile, filterContext.Exception.ToString());

            if (filterContext.HttpContext.IsCustomErrorEnabled)
            {
                filterContext.ExceptionHandled = true;
                this.View("UnexpectedError").ExecuteResult(this.ControllerContext);
            }
        }
    }
}
