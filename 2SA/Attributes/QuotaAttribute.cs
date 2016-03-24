using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eArchiver.Constants;
using System.Web.Routing;
using eArchiver.Models.Repositories.Quota;

namespace eArchiver.Attributes
{
    public class QuotaAttribute:FilterAttribute,IActionFilter
    {
        public QuotaTypes QuotaType { get; set; }
        #region IActionFilter Members

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
           // throw new NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            RouteValueDictionary routeValues = new RouteValueDictionary();
            routeValues.Add("action", "Index");
            routeValues.Add("controller", "Quota");

            int currentValue;
            int maxValue;
            int categoryID;
            int.TryParse(filterContext.RequestContext.HttpContext.Request["DictCategoryID"] ,out categoryID);
            bool deny = new QuotaRepository().QuotaExceeded(QuotaType,categoryID, out maxValue, out currentValue); 

            filterContext.Controller.TempData["quotaType"] = QuotaType;
            filterContext.Controller.TempData["currentValue"] = currentValue;
            filterContext.Controller.TempData["maxValue"] = maxValue;

            if (deny)
            {
                if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
                    filterContext.Controller.TempData["isAjax"]= true;
                else
                    filterContext.Controller.TempData["isAjax"] = false;
                  
                filterContext.Result = new RedirectToRouteResult(routeValues);
            }
        }

        #endregion
    }
}
