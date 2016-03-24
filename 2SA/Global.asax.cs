using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Net;

namespace eArchiver
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{htm}", new { htm = @".*\.htm(/.*)?" });
            routes.MapRoute(
                "Default",                                              // Route name
                "{controller}/{action}/{id}",                           // URL with parameters
                new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
            );

            routes.MapRoute(
                "DictionaryRoute",                                              // Route name
                "{controller}/{action}/{dictionary}",                           // URL with parameters
                new { controller = "Settings", action = "Dictionary" }  // Parameter defaults
            );

            routes.MapRoute("Catch All", "{*path}",

        new { controller = "Error", action = "Http404Error" });
        }

        protected void Application_Start()
        {
            RegisterRoutes(RouteTable.Routes);

            ModelValidatorProviders.Providers.Add(
             new LocalizedClientDataTypeModelValidatorProvider());
            RegisterGlobalFilters(GlobalFilters.Filters);
        }

        protected void Application_BeginRequest()
        {
            //HttpContext.Current.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            //HttpContext.Current.Response.Cache.SetValidUntilExpires(false);
            //HttpContext.Current.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            //HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //HttpContext.Current.Response.Cache.SetNoStore();
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
           
            filters.Add(new SetCultureFilter());
        }
    }
}