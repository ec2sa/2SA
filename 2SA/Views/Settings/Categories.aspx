<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<eArchiver.Models.Category>>" %>
<%@ Import Namespace="eArchiver.Helpers"%>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%= "2SA - S�ownik " + Html.Headers().Kategorii%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="height:50px;">
         <% Html.RenderPartial("SettingsSubsectionLinks"); %> 
    </div>
    <br />
    
    <div id="CategoriesContent">
        <%Html.RenderPartial("CategoriesUserControl", Model); %>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsContent" runat="server">
</asp:Content>
