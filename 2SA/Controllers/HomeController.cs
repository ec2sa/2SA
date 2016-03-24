using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eArchiver.Controllers;


public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            ViewData["Message"] = Resources.Strings.S118;
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Scans");
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult SampleTags()
        {
            return View();
        }


        [HttpPost]
        public ActionResult SetCulture(FormCollection items)
        {
            var originalurl = Request.UrlReferrer.OriginalString;
            HttpContext.Session["language"] = items["language"] == null ? "pl-PL" : items["language"].ToString();

            return new RedirectResult(originalurl, false);
        }
    }

