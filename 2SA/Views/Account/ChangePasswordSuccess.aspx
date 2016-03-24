<%@Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="changePasswordTitle" ContentPlaceHolderID="TitleContent" runat="server">
    2SA - <%= Resources.Strings.S119 %>
</asp:Content>

<asp:Content ID="changePasswordSuccessContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%= Resources.Strings.S119 %></h2>
    <p>
        <%= Resources.Strings.S107 %>
    </p>
</asp:Content>
