using System;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Collections.Generic;
using eArchiver.Models.Repositories.Shared;
using eArchiver.Controllers;
using eArchiver.Models;

namespace Helpers
{
    public static class NavigationContextHelper
    {
        public static bool IsCurrentSection(this HtmlHelper helper, string controllerName)
        {
            string currentControllerName = (string)helper.ViewContext.RouteData.Values["controller"];

            if (currentControllerName.Equals(controllerName, StringComparison.CurrentCultureIgnoreCase))
                return true;

            return false;
        }

        public static bool IsCurrentSubsection(this HtmlHelper helper, string actionName, string controllerName)
        {
            string currentControllerName = (string)helper.ViewContext.RouteData.Values["controller"];
            string currentActionName = (string)helper.ViewContext.RouteData.Values["action"];

            if (currentControllerName.Equals(controllerName, StringComparison.CurrentCultureIgnoreCase) && currentActionName.Equals(actionName, StringComparison.CurrentCultureIgnoreCase))
                return true;

            return false;
        }

        public static string SectionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, object routeValues, object htmlAttributes, string currentSectionClass,bool linkOnly)
        {
            //TODO: na razie olewamy htmlAttributes!!!

            if (!IsCurrentSection(htmlHelper, controllerName))
            {
                if (linkOnly)
                    return htmlHelper.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes).ToString();
                else
                {
                    string result = string.Format("<li>{0}</li>", htmlHelper.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes));
                    return result;
                }
            }
            else
            {
                if(linkOnly)
                    return htmlHelper.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes).ToString();
                else
                    return string.Format("<li class=\"{1}\">{0}</li>", htmlHelper.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes),currentSectionClass);
            }
        }

        public static string SubsectionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, object routeValues, object htmlAttributes, string currentSubsectionClass, bool linkOnly)
        {
            //TODO: na razie olewamy htmlAttributes!!!

            if (!IsCurrentSubsection(htmlHelper, actionName, controllerName))
            {
               
                if (linkOnly)
                    return htmlHelper.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes).ToString();
                else
                    return string.Format("<li>{0}</li>", htmlHelper.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes));


            }
            else
            {
               
                if (linkOnly)
                    return htmlHelper.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes).ToString();
                else
                    return string.Format("<li class=\"{1}\">{0}</li>", htmlHelper.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes), currentSubsectionClass);
            }

        }

        public static string ClientsDropDown(this HtmlHelper htmlHelper)
        {
            ClientsRepository clientRepository = new ClientsRepository();
            List<Client> clientsList = clientRepository.GetUserClients(AppContext.GetUserGuid());
            int selectedID = AppContext.GetCID();
            //if (clientsList.Count > 1)
            //{

                if (selectedID != -1)
                {
                    return htmlHelper.DropDownList("ClientsDropDown", new SelectList(clientsList, "ClientID", "ClientName", selectedID), new { onchange = "ChangeClient();", style = "width:170px;" }).ToString();
                }
                else
                {
                    return htmlHelper.DropDownList("ClientsDropDown", new SelectList(clientsList, "ClientID", "ClientName"), new { onchange = "ChangeClient();", style = "width:170px;" }).ToString();
                }
            //}
            //else
            //{
            //    return htmlHelper.Encode(clientRepository.GetClientName(selectedID));
            //}
            
        }

    }
}
