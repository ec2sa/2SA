<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<eArchiver.Models.DocumentHistory>>" %>
<%@ Import Namespace="eArchiver.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	2SA - Historia zmian dokumentu
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%= Resources.Strings.S97 %></h2>

    <% if (Model.Count() == 0)
       { %>
        <div class="infoMessage">
            Ten dokument nie by³ jeszcze edytowany.
        </div>
    <% }
       else
       { %>
        <table class="grid">
            <tr>
                <th><%= Resources.Strings.S98 %></th>
                <th><%= Resources.Strings.S99 %></th>
                <th><%= Resources.Strings.S29 %></th>                
                <th><%= Resources.Strings.S30 %></th>
                <th><%= Resources.Strings.S31 %></th>
                <th><%= Resources.Strings.S33 %></th>
                <th><%= Resources.Strings.S34 %></th>
                <th><%= Resources.Strings.S35 %></th>
                <th><%= Resources.Strings.S36 %></th>
                <th><%= Resources.Strings.S37 %></th>
                <th><%= Resources.Strings.S32 %></th>
            </tr>

        <% foreach (var item in Model)
           { %>
        
            <tr>
                <td><%= Html.Encode(String.Format("{0:g}", item.EntryDate))%></td>
                <td><%= Html.Encode(item.Editor.HasValue ? Membership.GetUser((object)item.Editor).UserName : String.Empty)%></td>
                <td><%= Html.Encode(item.DocumentNumber)%></td>
                <td><%= Html.Encode(item.CaseNumber)%></td>
                <td><%= Html.Encode(String.Format("{0:d}", item.Date))%></td>
                <td><%= Html.Encode(item.DocumentCategory)%></td>
                <td><%= Html.Encode(item.DocumentType)%></td>
                <td><%= Html.Encode(item.DocumentType2)%></td>
                <td><%= Html.Encode(item.Tags)%></td>
                <td><%= Html.Encode(item.Sender)%></td>
                <td><%= Html.Encode(item.Description)%></td>
            </tr>
        
        <% } %>
        </table>
    <% } %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsContent" runat="server">
</asp:Content>

