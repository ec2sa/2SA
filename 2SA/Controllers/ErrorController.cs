using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace eArchiver.Controllers
{
    public class ErrorController : BaseController
    {
        
        public ActionResult UnexpectedError()
        {
            return View();
        }

        public ActionResult Http404Error()
        {
            return View();
        }

    }
}
