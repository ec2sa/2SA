<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<eArchiver.Models.ViewModels.Account.EditGroupViewModel>" %>
<%@ Import Namespace="Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	2SA - Edycja grupy
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" language="javascript">
        $(document).ready(function() {
            $("#btnAdd").click(function() {
                $("#AllUsers option:selected").appendTo("#GroupUsers");
            });

            $("#btnAddAll").click(function() {
                $("#AllUsers > option").appendTo("#GroupUsers");
            });

            $("#btnRemove").click(function() {
                $("#GroupUsers option:selected").appendTo("#AllUsers");
            });

            $("#btnRemoveAll").click(function() {
                $("#GroupUsers > option").appendTo("#AllUsers");
            });

            //przed wys�aniem zaznacza wszystkie elementy z listy uzytkownik�w aby zosta�y przes�ane
            $("form").submit(function() {
                $("#GroupUsers *").attr("selected", "selected");
                return true;
            });


            $("#Submit").click(function() {
            
            });
        });              
    </script>
 <ul class="subsections">
        <li class="currentSubsection"><%= Html.ActionLink("Grupy", "Groups") %></li>
        <li><%= Html.ActionLink("U�ytkownicy", "Users") %></li>
        <li><%= Html.ActionLink("Klienci", "Clients") %></li> 
        <li><%= Html.ActionLink("Us�uga skan�w zdalnych", "SetRemoteScansUserPassword")%></li>
    </ul>

    <h2>Edytuj grup� <%=Html.Encode(Model.Group.GroupName) %></h2>
    
    <%using (Html.BeginForm())
    {%>
        <fieldset>
            <legend>Dane grupy</legend>
            <ol>
                <li>
                    <label for="GroupName">Nazwa</label>
                    <%string prefix=eArchiver.Controllers.AppContext.GetClientPrefix(Model.Group.ClientID); %>
                    <%=Html.UserNameTextBox("GroupName", prefix ,prefix!=null?Model.Group.GroupName.Replace(prefix,string.Empty):Model.Group.GroupName)%>
                </li>
                <li>
                    <%= Html.LabeledDropDownList("ClientID", new SelectList(Model.Clients, "ClientID", "ClientName", Model.Group.ClientID), "Klient") %>
                </li>
                <li>
                    <%= Html.LabeledTextArea("Description", "Opis", Model.Group.Description, 4, 45, null) %>
                </li>
            </ol>
        </fieldset>
        <fieldset>
            <legend>Uprawnienia</legend>
            <ol>
                <li>
                    <label for="Roles">Role grupy</label>
                    <div id="Roles" class="inputLike">
                      <%foreach (string role in Model.AllRoles)
                       {%>
                            <%= Html.CheckBox(role, Model.IsGroupRole(role)) %>
                            <label for="<%= Html.Encode(role) %>"><%= Html.Encode(role) %></label>
                            <br />      
                     <%} %>
                     </div>
                </li>
            
                <li>
                    <label for="ManageUsers">U�ytkownicy</label>
                    <div id="ManageUsers" class="inputLike"> 
                        <div class="left">
                            <%= Html.ListBox("AllUsers", new MultiSelectList(Model.NotInGroupUsers, "ProviderUserKey", "UserName"), new { size = 10, style = "width: 130px" })%>
                        </div> 
                        <div class="middle">
                            <a name="bottom" id="btnAddAll" href="#bottom">Dodaj wszystkie</a>
                            <a id="btnAdd" href="#bottom">Dodaj</a>
                            <a id="btnRemove" href="#bottom">Usu�</a>
                            <a id="btnRemoveAll" href="#bottom">Usu� wszystkie</a>
                        </div>
                        <div class="right">
                            <%= Html.ListBox("GroupUsers", new MultiSelectList(Model.GroupUsers, "ProviderUserKey", "UserName"), new { size = 10, style = "width: 130px" })%>
                        </div> 
                    </div>
                </li>
            </ol>
        </fieldset>
        <fieldset class="submit">
                <input name="Submit" type="submit" value="Zapisz" />
                <input name="Submit" type="submit" value="Anuluj" />
        </fieldset>
        <fieldset>
            <legend>Opis r�l</legend>
                <div class="padded">
                    <% if (User.IsInRole(eArchiver.Constants.RoleNames.ClientAdministrator))
                       { %>
                    <span style="margin:3px 0 0 0; display:block;">ClientAdministrator - uprawnia do zarz�dzania klientami</span>
                    <% } %>
                    <span style="margin:3px 0 0 0; display:block;">Administrator � uprawnia do zarz�dzania: definicjami  s�ownik�w/u�ytkownikami/ grupami/skanami/nag��wkami</span>
                    <span style="margin:3px 0 0 0; display:block;">Level1Full  - uprawnia do odczytu i zapisu: numer dokumentu/numer sprawy/data/ kategoria dokumentu/typ dokumentu</span>
                    <span style="margin:3px 0 0 0; display:block;">Level1Read � uprawnia do odczytu: numer dokumentu/numer sprawy/data/ kategoria dokumentu/typ dokumentu</span>
                    <span style="margin:3px 0 0 0; display:block;">Level2Full � uprawnia do odczytu i zapisu: nadawca/opis</span>
                    <span style="margin:3px 0 0 0; display:block;">Level2Read � uprawnia do odczytu: nadawca/opis</span>
                    <span style="margin:3px 0 0 0; display:block;">LevelContent � uprawnia do wyszukiwania po zawarto�ci skan�w</span>
                    <span style="margin:3px 0 0 0; display:block;">SkansFull � uprawnia do odczytu i zapisu: skany</span>
                    <span style="margin:3px 0 0 0; display:block;">SkansRead � uprawnia do odczytu: skany </span>

            </div>
        </fieldset>
    <%} %>
</asp:Content>
