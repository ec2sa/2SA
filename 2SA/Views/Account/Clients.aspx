<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.IEnumerable<eArchiver.Models.Client>>" %>
<%@ Import Namespace="eArchiver.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	2SA - Klienci
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <ul class="subsections">
        <li><%= Html.ActionLink("Grupy", "Groups") %></li>
        <li><%= Html.ActionLink("U¿ytkownicy", "Users") %></li>
        <li class="currentSubsection"><%= Html.ActionLink("Klienci", "Clients") %></li> 
        <li><%= Html.ActionLink("Us³uga skanów zdalnych", "SetRemoteScansUserPassword")%></li>
    </ul>
    
    <h2>Klienci</h2>

    <div class="padded">
        <%= Html.ActionLink("Dodaj klienta", "NewClient", null, new { @class="add iconLink"})%>
    </div>
    
    <table class="grid">
        <tr>
            <th>Aktywny</th>
            <th>Nazwa</th>
            <th>Prefix</th>
            <th>Opis</th>
            
        </tr>
        <%foreach (Client client in Model)
          {%>
            <tr>
                <td>
                    <%= Html.CheckBox(client.ClientID + "_active", client.IsActive, new { disabled="disabled" }) %>
                </td>
                <td>
                    <%= Html.ActionLink(client.ClientName, "EditClient", new { clientID = client.ClientID}) %>
                </td>
                <td><%= Html.Encode(client.ClientPrefix) %></td>
                <td><%= Html.EncodeLong(client.ClientDescription, 40) %></td>
            </tr>          
        <%} %>
        
    </table>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsContent" runat="server">
</asp:Content>
