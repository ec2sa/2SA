<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<eArchiver.Models.ViewModels.Account.EditUserViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	2SA - Edycja u¿ytkownika
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <script type="text/javascript" language="javascript">
     $(document).ready(function() {
         $("#lnkAddGroup").click(function() {

             if ($("#AllGroups option:selected").val().length > 0) 
             {
                 $("#AllGroups option:selected").appendTo("#UserGroups");
             }
         });

         $("#lnkRemoveSelected").click(function() {
             $("#UserGroups option:selected").appendTo("#AllGroups");
         });

         //przed wys³aniem zaznacza wszystkie elementy z listy uzytkowników aby zosta³y przes³ane
         $("form").submit(function() {
             $("#UserGroups *").attr("selected", "selected");
             return true;
         });


         $("#Submit").click(function() {

         });
     });              
    </script>
     <ul class="subsections">
        <li><%= Html.ActionLink("Grupy", "Groups") %></li>
        <li class="currentSubsection"><%= Html.ActionLink("U¿ytkownicy", "Users") %></li>
        <li><%= Html.ActionLink("Klienci", "Clients") %></li> 
        <li><%= Html.ActionLink("Us³uga skanów zdalnych", "SetRemoteScansUserPassword")%></li>
    </ul>
    <h2>Edytuj u¿ytkownika <%= Html.Encode(Model.User.UserName) %> </h2>
    
    <%using (Html.BeginForm())
    {%>
        <% if (TempData["NewPassword"] != null)
           { %>
                <div class="infoMessage">
                    <%=Html.Encode(string.Format("Nowe has³o: {0}",TempData["NewPassword"])) %>
                </div>
        <% } %>
        <fieldset>
            <legend>Konto u¿ytkownika</legend>
            <ol>
                <li>
                    <%= Html.LabeledTextBox("Login", "Login", Model.User.UserName, new { disabled = "dicabled" })%>
                </li>
                <li>
                    <%= Html.LabeledTextBox("Name", "Imiê i nazwisko", Model.User.Comment)%>
                </li>
                <li>
                    <%= Html.LabeledTextBox("Email", "Email", Model.User.Email)%>
                </li>
                <li>
                    <%= Html.CheckBox("Active", Model.User.IsApproved)%>
                    <label for="Active">Aktywny</label>
                </li>
                <li>
                    <%= Html.ActionLink("Resetuj has³o", "ResetPassword", new { id = Model.User.ProviderUserKey})%>
                </li>
            </ol>
        </fieldset>
        <fieldset>
            <legend>Grupy</legend>
            <ol>
                <li>
                    <label for="AllGroups">Grupy do którch u¿ytkownik nie nale¿y</label>
                    <%= Html.DropDownList("AllGroups", new SelectList(Model.NotUserGroups, "GroupId", "GroupName"), "- wybierz grupê -")%>
                    <a id="lnkAddGroup" href="#">Dodaj do grupy</a>
                </li>
                <li>
                    <%= Html.LabeledListBox("UserGroups", new SelectList(Model.UserGroups, "GroupId", "GroupName"), "Grupy u¿ytkownika", new { size = 5, style = "width: 180px" })%>
                    <a id="lnkRemoveSelected" href="#">Usuñ z wybranej grupy</a>
                </li>
            </ol>
        </fieldset>
        <fieldset class="submit">
            <input name="Submit" type="submit" value="Zapisz" />
            <input name="Submit" type="submit" value="Anuluj" />
        </fieldset>
    <%} %>
</asp:Content>
