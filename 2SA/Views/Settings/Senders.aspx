<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<eArchiver.Models.Sender>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	2SA - Nadawcy
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function() {
            initDialog();
        });
        
        function initDialog() {
            $("#senderDialog").dialog({
                bgiframe: true,
                height: 550,
                width: 500,
                modal: true,
                autoOpen: false,
                resizable: false
            });
        }
        $('.showDialog').live('click', initDialog);
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

   <% Html.RenderPartial("SettingsSubsectionLinks"); %> 
    <h2></h2>
    
    <div class="padded">
        <%= Html.ActionLink(Resources.Strings.S39, "NewSender", null, new { @class = "add iconLink" })%>
    </div>
    <table class="grid">
        <tr>
            <th><%= Resources.Strings.S44 %></th>
            <th><%= Resources.Strings.S45 %></th>
            <th><%= Resources.Strings.S46 %></th>
            <th></th>
        </tr>
        <%foreach (var sender in Model)
          {%>
            <tr>
                <td><%=Html.Encode(sender.FirstName) %></td>
                <td><%=Html.Encode(sender.LastName) %></td>
                <td><%=Html.Encode(sender.Company) %></td>
                <td>
                    <%= Html.ActionLink(Resources.Strings.S122, "EditSender", new { senderID = sender.SenderID })%>
                </td>
            </tr>
        <%} %>
        
    </table>
    
</asp:Content>

