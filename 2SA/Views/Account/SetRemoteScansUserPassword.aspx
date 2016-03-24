<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	2SA - Ustawianie has³a dla u¿ytkownika us³ugi RemoteScans
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <ul class="subsections">
        <li><%= Html.ActionLink("Grupy", "Groups") %></li>
        <li><%= Html.ActionLink("U¿ytkownicy", "Users") %></li>
        <li><%= Html.ActionLink("Klienci", "Clients") %></li> 
        <li class="currentSubsection"><%= Html.ActionLink("Us³uga skanów zdalnych", "SetRemoteScansUserPassword")%></li>
    </ul>
      
      <h2>Ustawianie has³a dla u¿ytkownika us³ugi RemoteScans</h2>
      
<%using (Html.BeginForm())
  { %>
  <input type="submit" value="Ustaw has³o" />
  <h3><% if (TempData["rsUserPwd"] != null)
        { %>
        Nowe has³o: <p class="error"><%=TempData["rsUserPwd"]%></p>
  <%} %>
  </h3>
<%} %>



</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsContent" runat="server">

</asp:Content>
