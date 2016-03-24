<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<eArchiver.Models.Header>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	2SA - Nag³ówki
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <% Html.RenderPartial("SettingsSubsectionLinks"); %> 
    <h2></h2>
<% using(Html.BeginForm("SaveHeaders","Settings")){ %>
    <table class="grid">
        <tr>
            <th>
                Nag³ówek
            </th>
            <th>
                Aktualna wartoœæ
            </th>
           
        </tr>

    <% int i = 0;
       foreach (var item in Model)
       { %>
    
        <tr>
           
            <td>
               <strong> <%= Html.Encode(item.HeaderKey)%></strong>
            </td>
            <td>
                <input type="hidden" name="headers.Index" value="<%=i.ToString() %>" />
                <input type="hidden" name="<%=string.Format("headers[{0}].HeaderKey",i) %>" value="<%= Html.Encode(item.HeaderKey)%>" />
                <input type="text" name="<%=string.Format("headers[{0}].HeaderValue",i) %>" value="<%= Html.Encode(item.HeaderValue)%>" />
                
            </td>
            
        </tr>
    
    <% i++;} %>

    </table>

   <br /><br />
        <input type="submit" value="Zapisz zmiany" />
        <%if(TempData["message"]!=null){ %>
         <p class="infoMessage"> <%=TempData["message"].ToString() %></p>
        <%} %>
        </div>
        
<%} %>   

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsContent" runat="server">
</asp:Content>

