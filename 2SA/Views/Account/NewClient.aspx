<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	2SA - Nowy klient
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

 <ul class="subsections">
        <li><%= Html.ActionLink("Grupy", "Groups") %></li>
        <li><%= Html.ActionLink("U¿ytkownicy", "Users") %></li>
        <li class="currentSubsection"><%= Html.ActionLink("Klienci", "Clients") %></li> 
        <li><%= Html.ActionLink("Us³uga skanów zdalnych", "SetRemoteScansUserPassword")%></li>
    </ul>
    <h2>Utwórz klienta</h2>
    
    
        <% using (Html.BeginForm())
           { %>
                <fieldset>
                    <legend>Dane klienta</legend>
                    <ol>
                        <li>
                            <%= Html.LabeledTextBox("ClientName", "Nazwa")%>
                            <%= Html.ValidationMessage("ClientName", new { style="vertical-align:text-top;" })%>
                        </li>
                        <li>
                            <%= Html.LabeledTextBox("ClientPrefix", "Prefiks",null,new{maxlength="10"})%>
                            <%= Html.ValidationMessage("ClientPrefix", new { style = "vertical-align:text-top;" })%>
                        </li>
                        <li>
                            <%= Html.LabeledTextArea("ClientDescription", "Opis")%>
                        </li>
                        <li>
                            <%= Html.CheckBox("IsActive", true)%>
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
