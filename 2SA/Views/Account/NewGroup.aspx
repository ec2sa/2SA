<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<eArchiver.Models.ViewModels.Account.EditGroupViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	2SA - Nowa grupa
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

            //przed wys³aniem zaznacza wszystkie elementy z listy uzytkowników aby zosta³y przes³ane
            $("form").submit(function() {
                $("#GroupUsers *").attr("selected", "selected");
                return true;
            });

            $("#ClientID").change(function() {
            $.get(rootPath + "/Account/GetPrefixForClient", { clientID: $(this).val() }, function(data) {
            $("#GroupNameprefix").text(data);
            },"text");
             });
            $("#Submit").click(function() {

            });
        });              
    </script>
 <ul class="subsections">
        <li class="currentSubsection"><%= Html.ActionLink("Grupy", "Groups") %></li>
        <li><%= Html.ActionLink("U¿ytkownicy", "Users") %></li>
        <li><%= Html.ActionLink("Klienci", "Clients") %></li> 
        <li><%= Html.ActionLink("Us³uga skanów zdalnych", "SetRemoteScansUserPassword")%></li>
    </ul>
    <h2>Utwórz grupê</h2>

    <%using (Html.BeginForm())
    {%>
        <fieldset>
            <legend>Dane grupy</legend>
            <ol>
                <li>
                    <label for="GroupName">Nazwa</label>
                    <%= Html.UserNameTextBox("GroupName", eArchiver.Controllers.AppContext.GetClientPrefix(), string.Empty)%>
                    <%= Html.ValidationMessage("userNameTextBoxName")%>
                </li>
                <li>
                    <%= Html.LabeledDropDownList("ClientID", new SelectList(Model.Clients, "ClientID", "ClientName", eArchiver.Controllers.AppContext.GetCID()), "Klient") %>
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
                    <label for="ManageUsers">U¿ytkownicy</label>
                    <div id="ManageUsers" class="inputLike"> 
                        <div class="left">
                        <%= Html.ListBox("AllUsers", new MultiSelectList(Model.NotInGroupUsers, "ProviderUserKey", "UserName"), new { size = 10, style = "width: 130px" })%>
                        </div> 
                        <div class="middle">
                            <a id="btnAddAll">Dodaj wszystkie</a>
                            <a id="btnAdd">Dodaj</a>
                            <a id="btnRemove">Usuñ</a>
                            <a id="btnRemoveAll">Usuñ wszystkie</a>
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
            <legend>Opis ról</legend>
                <div class="padded">
                    <% if (User.IsInRole(eArchiver.Constants.RoleNames.ClientAdministrator))
                       { %>
                    <span style="margin:3px 0 0 0; display:block;">ClientAdministrator - uprawnia do zarz¹dzania klientami</span>
                    <% } %>
                    <span style="margin:3px 0 0 0; display:block;">Administrator – uprawnia do zarz¹dzania: definicjami  s³owników/u¿ytkownikami/ grupami/skanami/nag³ówkami</span>
                    <span style="margin:3px 0 0 0; display:block;">Level1Full  - uprawnia do odczytu i zapisu: numer dokumentu/numer sprawy/data/ kategoria dokumentu/typ dokumentu</span>
                    <span style="margin:3px 0 0 0; display:block;">Level1Read – uprawnia do odczytu: numer dokumentu/numer sprawy/data/ kategoria dokumentu/typ dokumentu</span>
                    <span style="margin:3px 0 0 0; display:block;">Level2Full – uprawnia do odczytu i zapisu: nadawca/opis</span>
                    <span style="margin:3px 0 0 0; display:block;">Level2Read – uprawnia do odczytu: nadawca/opis</span>
                    <span style="margin:3px 0 0 0; display:block;">LevelContent – uprawnia do wyszukiwania po zawartoœci skanów</span>
                    <span style="margin:3px 0 0 0; display:block;">SkansFull – uprawnia do odczytu i zapisu: skany</span>
                    <span style="margin:3px 0 0 0; display:block;">SkansRead – uprawnia do odczytu: skany </span>

            </div>
        </fieldset>
        
    <%} %>

</asp:Content>
