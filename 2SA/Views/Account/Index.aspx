<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	2SA - Zarz¹dzanie u¿ytkownikami
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <ul class="subsections">
        <li><%= Html.ActionLink("Grupy", "Groups") %></li>
        <li><%= Html.ActionLink("U¿ytkownicy", "Users") %></li>
        <%if (eArchiver.Controllers.AppContext.IsClientAdmin())
          { %>
        <li><%= Html.ActionLink("Klienci", "Clients")%></li> 
        <%} %>
        <li><%= Html.ActionLink("Us³uga skanów zdalnych", "SetRemoteScansUserPassword")%></li> 
    </ul>
    <h3>
        Zarz¹dzaj grupami/u¿ytkownikami
    </h3>
</asp:Content>
