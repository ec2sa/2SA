<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<eArchiver.Models.ViewModels.Account.UsersViewModel>" %>
<%@ Import Namespace="Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	2SA - U¿ytkownicy
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <ul class="subsections">
        <li><%= Html.ActionLink("Grupy", "Groups") %></li>
        <li class="currentSubsection"><%= Html.ActionLink("U¿ytkownicy", "Users") %></li>
        <li><%= Html.ActionLink("Klienci", "Clients") %></li> 
        <li><%= Html.ActionLink("Us³uga skanów zdalnych", "SetRemoteScansUserPassword")%></li>
    </ul>
    
    <h2>U¿ytkownicy 
        <% if (!string.IsNullOrEmpty(Model.ClientName))
           {%>
                <%= Html.Encode(string.Concat("(",Model.ClientName,")")) %>   
        <% } %>
    </h2>
    
    <div class="padded">
       <%= Html.ActionLink("Utwórz u¿ytkownika", "NewUser", null, new { @class = "add iconLink" })%>
    </div>
    
    <table class="grid">
        <tr>
            <th>
                Login
            </th>
            <th>
                Imiê i nazwisko
            </th>
            <th>
                Email
            </th>
            <th>
                Aktywny
            </th>
        </tr>
        <%foreach (MembershipUser user in Model.Users)
          {%>
              <tr>
                <td>
                    <%= Html.ActionLink(user.UserName, "EditUser", new { id = user.ProviderUserKey })%>
                </td>
                <td>
                    <%= Html.Encode(user.Comment) %>
                </td>
                <td>
                    <%= Html.Encode(user.Email) %>
                </td>
                <td>
                    <%= Html.IsActiveText(user.IsApproved) %>
                </td>
            </tr>
        <%} %>
        
    </table>

</asp:Content>
