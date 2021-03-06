<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	2SA - Zarządzanie użytkownikami
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <ul class="subsections">
        <li><%= Html.ActionLink("Grupy", "Groups") %></li>
        <li><%= Html.ActionLink("Użytkownicy", "Users") %></li>
        <%if (eArchiver.Controllers.AppContext.IsClientAdmin())
          { %>
        <li><%= Html.ActionLink("Klienci", "Clients")%></li> 
        <%} %>
        <li><%= Html.ActionLink("Usługa skanów zdalnych", "SetRemoteScansUserPassword")%></li> 
    </ul>
    <h3>
        Zarządzaj grupami/użytkownikami
    </h3>
</asp:Content>
