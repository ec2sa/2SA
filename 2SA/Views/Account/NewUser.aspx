<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<eArchiver.Models.ViewModels.Account.EditUserViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	2SA - Nowy u�ytkownik
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

         //przed wys�aniem zaznacza wszystkie elementy z listy uzytkownik�w aby zosta�y przes�ane
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
        <li class="currentSubsection"><%= Html.ActionLink("U�ytkownicy", "Users") %></li>
        <li><%= Html.ActionLink("Klienci", "Clients") %></li> 
        <li><%= Html.ActionLink("Us�uga skan�w zdalnych", "SetRemoteScansUserPassword")%></li>
    </ul>
    <h2>Utw�rz u�ytkownika</h2>
    
    <%= Html.ValidationSummary() %>
    
    <%using (Html.BeginForm())
    {%>
        <fieldset>
            <legend>Konto u�ytkownika</legend>
            <ol>
                <li>
                    <label for="Login">Login</label>
                    <%=Html.UserNameTextBox("Login",eArchiver.Controllers.AppContext.GetClientPrefix(),string.Empty) %>
                </li>
                <li>
                    <%= Html.LabeledTextBox("Name", "Imi� i nazwisko")%>
                </li>
                <li>
                    <%= Html.LabeledTextBox("Email", "Email")%>
                </li>
                <li>
                    <label for="Password">Has�o</label>
                    <%= Html.Password("Password") %>
                </li>
                <li>
                    <label for="RepeatPassword">Powt�rz has�o</label>
                    <%= Html.Password("RepeatPassword") %>
                </li>
                <li>
                    <%= Html.CheckBox("Active", true)%>
                    <label for="Active">Aktywny</label>
                </li>
            </ol>
        </fieldset>
        <fieldset>
            <legend>Grupy</legend>
            <ol>
                <li>
                    <label for="AllGroups">Grupy do kt�rch u�ytkownik nie nale�y</label>
                    <%= Html.DropDownList("AllGroups", new SelectList(Model.NotUserGroups, "GroupId", "GroupName"), "- wybierz grup� -")%>
                    <a id="lnkAddGroup">Dodaj do grup u�ytkownika</a>
                </li>
                <li>
                    <%= Html.LabeledListBox("UserGroups", new SelectList(Model.UserGroups, "GroupId", "GroupName"), "Grupy u�ytkownika", new { size = 5, style = "width: 180px" })%>
                    <a id="lnkRemoveSelected">Usu� z wybranej grupy</a>
                </li>
            </ol>
        </fieldset>
        <fieldset class="submit">
            <input name="Submit" type="submit" value="Zapisz" />
            <input name="Submit" type="submit" value="Anuluj" />
        </fieldset>
    <%} %>
</asp:Content>
