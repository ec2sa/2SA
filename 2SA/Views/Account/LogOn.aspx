<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="loginTitle" ContentPlaceHolderID="TitleContent" runat="server">
    2SA - <%= Resources.Strings.S3 %>
</asp:Content>

<asp:Content ID="loginContent" ContentPlaceHolderID="MainContent" runat="server">
    
    <%= Html.ValidationSummary(Resources.Strings.S108) %>

    <% using (Html.BeginForm()) { %>
        <fieldset>
            <legend><%= Resources.Strings.S3 %></legend>
            <ol>
            <li>
                <label for="username"><%= Resources.Strings.S2 %>:</label>
                <%= Html.TextBox("username") %>
                <%= Html.ValidationMessage("username") %>
            </li>
            <li>
                <label for="password"><%= Resources.Strings.S4 %>:</label>
                <%= Html.Password("password") %>
                <%= Html.ValidationMessage("password") %>
            </li>
            <li>
                <%= Html.CheckBox("rememberMe") %> <label class="inline" for="rememberMe"><%= Resources.Strings.S5 %></label>
            </li>
            <fieldset class="submit">
                <input type="submit" value='<%= Resources.Strings.S1 %>' />
            </fieldset>
            
         
          </ol>
        </fieldset>
    <% } %>
</asp:Content>
