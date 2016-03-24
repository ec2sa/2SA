<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<eArchiver.Models.ViewModels.Account.GroupsViewModel>" %>
<%@ Import Namespace="eArchiver.Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	2SA - Grupy
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <ul class="subsections">
        <li class="currentSubsection"><%= Html.ActionLink("Grupy", "Groups") %></li>
        <li><%= Html.ActionLink("U¿ytkownicy", "Users") %></li>
        <li><%= Html.ActionLink("Klienci", "Clients") %></li> 
        <li><%= Html.ActionLink("Us³uga skanów zdalnych", "SetRemoteScansUserPassword")%></li>
    </ul>
    
    <h2>Grupy</h2>
    
    <div class="padded">
        <%= Html.ActionLink("Utwórz grupê", "NewGroup", null, new { @class = "add iconLink" })%>
    </div>
    
    <table class="grid">
        <tr>
            <th>
                Nazwa
            </th>
            <th>
                Klient
            </th>
            <th>
                Opis
            </th>
        </tr>
        <%foreach (eArchiver.Models.Group group in Model.Groups)
          {%>
              <tr>
                <td>
                    <%= Html.ActionLink(group.GroupName, "EditGroup", new { id = group.GroupId })%>
                </td>
                <td>
                    <%= Html.Encode(group.Client.ClientName) %>
                </td>
                <td>
                    <%= Html.EncodeLong(group.Description, 40) %>
                </td>
            </tr>
        <%} %>
        
    </table>

</asp:Content>
