<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%
    if (Request.IsAuthenticated) {
%>      
            <%= Resources.Strings.S6 %> <b><%= Html.Encode(Page.User.Identity.Name) %></b>!
            &nbsp;
            [ <%= Html.ActionLink(Resources.Strings.S7, "ChangePassword", "Account")%> ]
            [ <%= Html.ActionLink(Resources.Strings.S8, "LogOff", "Account")%> ]
            
        &nbsp;
            <%= Resources.Strings.S9 %>:
            <%= Html.ClientsDropDown() %>
        
        
<%
    }
    else {
%> 

        [ <%= Html.ActionLink(Resources.Strings.S1, "LogOn", "Account")%> ]
<%
    }
%>
                      <%
    var items=new List<SelectListItem>();
    items.Add(new SelectListItem(){Value="pl",Text="Język polski",Selected=System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToUpper()=="PL"});
   // items.Add(new SelectListItem() { Value = "en", Text = "English", Selected = System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToUpper() == "EN" });
    items.Add(new SelectListItem() { Value = "it", Text = "Lingua italiana", Selected = System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToUpper() == "IT" });
%>
 <% using (Html.BeginForm("SetCulture", "Home", FormMethod.Post, new { name = "languageVersion",style="display:inline" })){ %>
<% =Html.DropDownList("language", items, new { style = "display:inline;" })%>
<%} %>
