<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<eArchiver.Models.Category>>" %>

<script type="text/javascript">
    function deleteCategory(id) {
        $.getJSON(rootPath+"/Custom/DeleteCategory?categoryID=" + id,
            function(data) {
                if (data == true) {
                    $("#trCategoryID_" + id).fadeOut("slow");
                }
            });

        }
        function clearCategory() {
            alert("cleaer");
            $("#CategoryName").val("");
    }
    
</script>

    <%using (Ajax.BeginForm("AddCategory", new AjaxOptions() { HttpMethod = "POST", UpdateTargetId = "CategoriesContent" }))
    { %>
        
        <table class="grid">
            <tr>
                <%--<th></th>--%>
                <th>
                    Kategoria
                </th>
                <th></th>
            </tr>

        <% foreach (var item in Model) { %>
        
            <tr id="trCategoryID_<%=Html.Encode(item.CategoryID) %>">
                <%--<td>
                    <%= Html.ActionLink("Usuñ", "Details", new { id=item.CategoryID })%>
                </td>--%>
                <td>
                    <%= Html.Encode(item.Name) %>
                </td>
                <td>
                    <a href="#" onclick="deleteCategory(<%= Html.Encode(item.CategoryID) %>)">x</a>
                </td>
                
            </tr>
        
        <% } %>
            <tr>
                <td>
                    <%=Html.TextBox("CategoryName")%>
                </td>
                <td>   
                    <input type="Submit" value="Dodaj" />
                </td>
            </tr>
        </table>
    <%} %>

