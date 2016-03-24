<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<eArchiver.Models.ViewModels.Scans.ScansBinViewModel>" %>
<%@ Import Namespace="eArchiver.Controllers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	2SA - Kosz skanów
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function() {

            $(".restoreScan").click(function() {
                var link = $(this);
                var scanGuid = $('input[type="hidden"]:first', link.parent()).val();
                $.getJSON(rootPath+"/Custom/RestoreScanFromRecycleBin?scanID=" + scanGuid, function(data) {
                    if (data == true) {
                        link.parent().fadeOut("slow");
                    }
                });
            });

            $(".deleteScan").click(function() {
                var link = $(this);
                var scanGuid = $('input[type="hidden"]:first', link.parent()).val();
                $.getJSON(rootPath+"/Custom/DeleteScanFromRecycleBin?scanID=" + scanGuid, function(data) {
                    if (data == true) {
                        link.parent().fadeOut("slow");
                    }
                    else {
                        alert('<%= Resources.Strings.S114 %>');
                    }
                });
            });
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%= Resources.Strings.S24 %></h2>
    <%= Html.ActionLink(Resources.Strings.S72, "Index") %>
    <%int totalCount = Model.ScansList.Count;
       int columns = 4;
       int rows = totalCount / columns;
       if (totalCount % columns != 0)
           rows++;
       int iterator = 0;  %>
       
       <table id="scansGrid" class="center">
           <%for (int i = 0; i < rows; i++)
             {%>
                <tr>
                    <% for (int j = 0; j < columns; j++)
                       {
                           if (iterator < totalCount)
                           {
                               Guid guid = Model.ScansList[iterator];
                               string fileName = Model.GetFileName(guid);
                               DateTime importDate = Model.GetImportDate(guid);
                        %>
			            <td>
			                <div class="scansBinItem">
                                <input type="hidden" value='<%=Html.Encode(guid) %>' />
                                
                                <img src=' <%=Url.Action("Preview", "Scans", new{scanGuid=guid}) %>' 
                                    alt='<%=Resources.Strings.S113 %> <%=Html.Encode(fileName) %> '/>
                                
                                <a href='<%=Url.Action("Download", "Scans", new{scanGuid=guid}) %>'
                                 ><%= Resources.Strings.S21 %></a>
                                <br />
                                <%if (AppContext.IsAdmin())
                                  { %>
                                    <a class="restoreScan"><%= Resources.Strings.S73 %></a> 
                                    <br />
                                    <a class="deleteScan"><%= Resources.Strings.S74 %></a> 
                                <%} %>
                                <p>
                                    <strong><%=Html.Encode(fileName)%></strong>
                                </p>
                                <p>
                                    <%=Html.Encode(string.Format("{1}: {0:yyyy-MM-dd} {2}. {0:hh:mm}", importDate, Resources.Strings.S75, Resources.Strings.S76))%>
                                </p>
                            </div>
			            </td>
			      <%    } 
                        iterator++;
                    } 
             }%>
			</tr>
		</table>
    
</asp:Content>


