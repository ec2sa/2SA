<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<eArchiver.Models.Type2>>" %>


<script type="text/javascript">
    function deleteType2(id) {
        $.getJSON(rootPath+"/Custom/DeleteType2?type2ID=" + id,
            function(data) {
                if (data == true) {
                    $("#trCategoryID_" + id).fadeOut("slow");
                }
            });

        }
        function clearCategory() {
           
            $("#CategoryName").val("");
    }
    
</script>

    <%using (Ajax.BeginForm("AddType2", new AjaxOptions() { HttpMethod = "POST", UpdateTargetId = "CategoriesContent" }))
    { %>
        
        <table class="grid">
            <tr>
                <%--<th></th>--%>
                <th>
                    Typ
                </th>
                <th></th>
            </tr>

        <% foreach (var item in Model) { %>
        
            <tr id="trCategoryID_<%=Html.Encode(item.Type2ID) %>">
                <%--<td>
                    <%= Html.ActionLink("Usuñ", "Details", new { id=item.CategoryID })%>
                </td>--%>
                <td>
                    <%= Html.Encode(item.Name) %>
                </td>
                <td>
                    <a href="#" onclick="deleteType2(<%= Html.Encode(item.Type2ID) %>)">x</a>
                </td>
                
            </tr>
        
        <% } %>
            <tr>
                <td>
                    <%=Html.TextBox("Type2Name")%>
                </td>
                <td>   
                    <input type="Submit" value="Dodaj" />
                </td>
            </tr>
        </table>
    <%} %>

