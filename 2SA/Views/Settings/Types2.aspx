<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<eArchiver.Models.Type2>>" %>
<%@ Import Namespace="eArchiver.Helpers"%>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%= "2SA - S³ownik " + Html.Headers().Typow%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="height:50px;">
         <% Html.RenderPartial("SettingsSubsectionLinks"); %> 
    </div>
    <br />
    
    <div id="CategoriesContent">
        <%Html.RenderPartial("Types2UserControl", Model); %>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsContent" runat="server">
</asp:Content>