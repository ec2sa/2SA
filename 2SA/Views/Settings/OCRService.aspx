<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<eArchiver.Models.pOCRGetConfigurationResult>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	2SA - Konfiguracja us³ugi OCR
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   <% Html.RenderPartial("SettingsSubsectionLinks"); %> 

<% if(ViewData["statusClass"]!=null){ %>
<h3>
<strong>Status: </strong><span class='<%=ViewData["statusClass"].ToString() %>'><%=ViewData["serviceStatus"].ToString() %></span>
</h3>
<%} %>

    <%= Html.ValidationSummary("Nie uda³o siê zapisaæ konfiguracji") %>

    <% using (Html.BeginForm()) {%>
        <fieldset>
            <legend>Godziny dzia³ania us³ugi OCR</legend>
            <ol>
                <li>
                    <%=Html.CheckBox("OCREnabled",Model.OCREnabled=="1"?true:false) %>
                    <label class="inline" for="OCREnabled">System 2SA ma korzystaæ z us³ugi OCR</label>
                </li>
                <li>
                    <hr />
                </li>
                <li>
                    <label for="OCRStartHour">Godzina pocz¹tkowa:</label>
                    <%= Html.TextBox("OCRStartHour", Model.OCRStartHour) %>
                    <%= Html.ValidationMessage("OCRStartHour", "*") %>
                </li>
                <li>
                    <label for="OCREndHour">Godzina koñcowa:</label>
                    <%= Html.TextBox("OCREndHour", Model.OCREndHour) %>
                    <%= Html.ValidationMessage("OCREndHour", "*") %>
                </li>
            </ol>
            <fieldset class="submit">
                <input type="submit" value="Zapisz" />
            </fieldset>
        </fieldset>

    <% } %>


</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsContent" runat="server">
</asp:Content>

