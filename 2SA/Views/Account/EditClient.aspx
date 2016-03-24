<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<eArchiver.Models.Client>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	2SA - Edycja klienta
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

 <ul class="subsections">
        <li><%= Html.ActionLink("Grupy", "Groups") %></li>
        <li><%= Html.ActionLink("U¿ytkownicy", "Users") %></li>
        <li class="currentSubsection"><%= Html.ActionLink("Klienci", "Clients") %></li> 
        <li><%= Html.ActionLink("Us³uga skanów zdalnych", "SetRemoteScansUserPassword")%></li>
    </ul>
    <h2>Edytuj klienta <%= Html.Encode(Model.ClientName) %></h2>
    
        <% using (Html.BeginForm())
           { %>
            <fieldset>
                <legend>Dane klienta</legend>
                <ol>
                    <li>
                        <%= Html.LabeledTextBox("ClientName", "Nazwa", Model.ClientName)%>
                        <%= Html.ValidationMessage("ClientName", new { style="vertical-align:text-top;" })%>
                    </li>
                    <li>
                        <%= Html.LabeledTextBox("ClientPrefix", "Prefiks", Model.ClientPrefix, new { disabled = "disabled" })%>
                        <%= Html.ValidationMessage("ClientPrefix", new { style = "vertical-align:text-top;" })%>
                    </li>
                    <li>
                        <%= Html.LabeledTextArea("ClientDescription", "Opis", Model.ClientDescription)%>
                    </li>
                    <li>
                        <%= Html.CheckBox("IsActive", Model.IsActive)%>
                        <label for="IsActive" class="inline">Aktywny</label>
                    </li>
                </ol>
            </fieldset>
            <fieldset class="submit">
                <input name="Submit" type="submit" value="Zapisz" />
                <input name="Submit" type="submit" value="Anuluj" />
                <%= Html.ValidationMessage("AllClient", new { style="vertical-align:text-top;" })%>    
            </fieldset>
        <% } %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsContent" runat="server">
</asp:Content>
