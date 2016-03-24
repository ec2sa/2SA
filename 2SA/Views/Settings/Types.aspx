<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<eArchiver.Models.ViewModels.Settings.TypesDictViewModel>" %>
<%@ Import Namespace="eArchiver.Helpers"%>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%= "2SA - S³ownik " + Html.Headers().Rodzajow%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div style="height:50px;">
         <% Html.RenderPartial("SettingsSubsectionLinks"); %> 
    </div>
    <br />

    <div id="TypesContent">
        <% Html.RenderPartial("TypesUserControl", Model);%>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsContent" runat="server">
</asp:Content>
