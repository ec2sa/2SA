<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<eArchiver.Models.ViewModels.Scans.ScanSelectorViewModel>" %>
<%@ Import Namespace="eArchiver.Controllers" %>
<%@ Import Namespace="eArchiver.Helpers" %>

     <script type="text/javascript">

         function selectorClick() {
             //dodanie wersji
             if ($("#scansDialog #originalScanID").val().length > 0) {
                 var originalId = $("#scansDialog #originalScanID").val();
                 var versionId = $('input[type="hidden"]:first', $(this).parent()).val();
                 var docId = $("#currentDocumentId").val();
                 $.post(rootPath + "/Custom/AddScanVersion",
                     { originalScanId: originalId, versionScanId: versionId, documentId: docId },
                     function() {
                         location.reload();
                     });
                 
             }
             else {//dodanie skanu
                 addScanFromGrid($(this));
             }
         }
         
         $(".scanSelector").unbind("click", "selectorClick");
         $(".scanSelector").click(selectorClick);

         $("#refresh").click(function() {
             $.ajax({
                 url: rootPath + "/Custom/RefreshAvailableScans",
                 cache: false,
                 dataType: "json",
                 success: function(data) {
                     $("#scansCount").text(data[0] + "/" + data[1]);
                 }
             });

         });
     </script>
     
     <%= Html.Hidden("scansToAdd") %>
     <input type="hidden" id="originalScanID" />
     
     <ul id="tmpul" style="display:none;">
        <li><div></div></li>
     </ul>
     
     <%int totalCount = Model.AvailableScans.Count;
       int columns = 3;
       int rows = totalCount / columns;
       if (totalCount % columns != 0)
           rows++;
       int iterator = 0;  %>
       <div class="scansInfo">
            <%=Resources.Strings.S20 %>: 
            <span id="scansCount">
                <%=string.Format("{0}/{1}", Model.KnownScansCount, Model.AllScansCount)%>
            </span>
            &nbsp;&nbsp;
             
            <span id="getLink">
                <%=Html.ActionLink(Resources.Strings.S21, "GetScans", "Documents", new {documentID = Request.QueryString["documentID"] }, null)%>
            </span>
            
            &nbsp; 
            <a id="refresh" href="#"><%=Resources.Strings.S22 %></a>&nbsp;&nbsp;&nbsp;&nbsp;
            <%if (AppContext.IsAdmin())
              { %>
                    <%= Html.ActionLink(Resources.Strings.S24, "ScansBin", "Scans")%>
            <%} %>
        </div>
       <table id="scansGrid" class="center">
           <%for (int i = 0; i < rows; i++)
            {%>
                <tr>
                    <% for (int j = 0; j < columns; j++)
			        {
                        if(iterator<totalCount)
                        {
                            var item = Model.AvailableScans[iterator];
                            %>
                            <td>
                                <div class="scansGridItem">
                                    <input type="hidden" value='<%=Html.Encode(item.ScanID) %>' />
                                    
                                    <img src=' <%=Url.Action("Preview", "Scans", new{scanGuid=item.ScanID}) %>' 
                                        alt='<%= Resources.Strings.S113 %> <%=Html.Encode(item.FileName) %> '/>
                                    
                                    <a href='<%=Url.Action("Download", "Scans", new{scanGuid=item.ScanID}) %>'
                                     title='<%=Html.Encode(item.FileName) %>'><%= Resources.Strings.S21%></a>
                                    <br />
                                    <a class="scanSelector" title='<%=Html.Encode(item.FileName) %>'><%= Resources.Strings.S39%></a> 
                                    <p>
                                        <strong><%=Html.Encode(item.FileName)%></strong>
                                    </p>
                                    <p>
                                        <%=Html.Encode(string.Format("{1}: {0:yyyy-MM-dd} {2}. {0:hh:mm}", item.ImportDate, Resources.Strings.S75, Resources.Strings.S76))%>
                                    </p>
                                </div>
                            </td>
			        <%  }
                        iterator++;
                    } %>
                </tr>
               
           <%}%>
        
       </table>
