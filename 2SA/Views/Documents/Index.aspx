<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<eArchiver.Models.ViewModels.Documents.DocumentsViewModel>" %>
<%@ Import Namespace="eArchiver.Helpers"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	2SA - <%=Resources.Strings.S29%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2></h2>
    <% Html.RenderPartial("DocumentSearchUserControl", Model.DocumentSearchModel); %>
    
</asp:Content>
