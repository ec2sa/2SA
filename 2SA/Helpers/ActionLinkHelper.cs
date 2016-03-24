using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Mvc;

namespace Helpers
{
    public static class ActionLinkHelper
    {
        public static string ActionLink(HtmlHelper htmlHelper, string linkText, string actionName) { return null; }

        internal static RouteValueDictionary MergeRouteValueDictionaries(RouteValueDictionary routeValueDictionary1, RouteValueDictionary routeValueDictionary2)
        {
            var _mergedRouteValues = new RouteValueDictionary();

            if ((routeValueDictionary1 != null) & (routeValueDictionary2 != null))
            {
                foreach (KeyValuePair<string, object> routeElement in routeValueDictionary1)
                {
                    _mergedRouteValues[routeElement.Key] = routeElement.Value;
                }

                foreach (KeyValuePair<string, object> routeElement in routeValueDictionary2)
                {
                    _mergedRouteValues[routeElement.Key] = routeElement.Value;
                }

                return _mergedRouteValues;
            }

            return null;
        }

    }
}
