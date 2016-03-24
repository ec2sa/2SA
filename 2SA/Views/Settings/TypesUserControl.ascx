<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<eArchiver.Models.ViewModels.Settings.TypesDictViewModel>" %>
<%@ Import Namespace="eArchiver.Helpers" %>
<script type="text/javascript">
    $(document).ready(function() {
        $("#DictCategoryID").live("change",
            function() { changeDictCategoryID(); }
        );
//        $("#DictCategoryID").livequery("change",
//            function() { changeDictCategoryID(); }
//        );
        
        
    });
    

</script>

<div>
    <%using (Ajax.BeginForm("AddType", new AjaxOptions() { HttpMethod = "POST", UpdateTargetId = "TypesContent" }))
      {%>
        <div class="padded">
            <%=Html.Headers().Kategoria%>:
            <%= Html.DropDownList("DictCategoryID", new SelectList(Model.Categories, "CategoryID", "Name", Model.SelectedCategory), Resources.Strings.S120)%>
        </div>
        
        <table id="TypesTable" class="grid">
            <tr>
                <th>
                    <%=Html.Headers().Rodzaj%>:
                </th>
                <th></th>
            </tr>
            <%
            if (Model.Types.Count > 0)
            {
                foreach (var item in Model.Types)
                {
                      %>
                      <tr id="trTypeID_<%=Html.Encode(item.TypeID)%>">
                        <td>
                            <%=Html.Encode(item.Name)%>
                        </td>
                        <td>
                            <a href="#" onclick="deleteType(<%= Html.Encode(item.TypeID) %>)">x</a>
                        </td>
                      </tr>
                <%}%>
              
            <%} %>
            <tr>
                <td>
                    <%= Html.TextBox("TypeName")%>
               </td>
               <td>     
                    <input type="submit" value="Dodaj" />
                </td>
            </tr>
        </table>
<%} %>

</div>