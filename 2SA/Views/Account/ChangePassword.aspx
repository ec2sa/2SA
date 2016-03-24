<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="changePasswordTitle" ContentPlaceHolderID="TitleContent" runat="server">
    2SA - <%= Resources.Strings.S119 %>
</asp:Content>

<asp:Content ID="changePasswordContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%= Resources.Strings.S7 %></h2>
    
    <p>
        <%= Resources.Strings.S10 %> <%=Html.Encode(ViewData["PasswordLength"])%>.
    </p>
    <%= Html.ValidationSummary(Resources.Strings.S106)%>

    <% using (Html.BeginForm()) { %>
        <div>
            <fieldset>
                <p>
                    <label for="currentPassword"><%= Resources.Strings.S11 %>:</label>
                    <%= Html.Password("currentPassword") %>
                    <%= Html.ValidationMessage("currentPassword") %>
                </p>
                <p>
                    <label for="newPassword"><%= Resources.Strings.S12 %>:</label>
                    <%= Html.Password("newPassword") %>
                    <%= Html.ValidationMessage("newPassword") %>
                </p>
                <p>
                    <label for="confirmPassword"><%= Resources.Strings.S13 %>:</label>
                    <%= Html.Password("confirmPassword") %>
                    <%= Html.ValidationMessage("confirmPassword") %>
                </p>
                <p>
                    <input type="submit" value="<%= Resources.Strings.S14 %>" />
                </p>
            </fieldset>
        </div>
    <% } %>
</asp:Content>
