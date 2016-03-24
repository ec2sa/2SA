<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<eArchiver.Models.ViewModels.Documents.DocumentDetailsViewModel>" %>
<%@ Import Namespace="eArchiver.Controllers" %>
<%@ Import Namespace="eArchiver.Models" %>
<%@ Import Namespace="eArchiver.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	2SA - <%=Resources.Strings.S29 %>
</asp:Content>

<asp:Content ID="ScriptsCont" ContentPlaceHolderID="ScriptsContent" runat="server">
    <script type="text/javascript">

        $(document).ready(
        function(){
            initDialog();
            }
        );
        
        function setPreview(scanGuid) {
            if(rootPath == "/")
                $('#selectedScanPreview').attr('src', '/Scans/Zoom?scanGuid=' + scanGuid);
            else
                $('#selectedScanPreview').attr('src', rootPath + '/Scans/Zoom?scanGuid=' + scanGuid);
        }

        function initDialog() {
            $("#senderDetailsDialog0,#senderDetailsDialog1,#senderDetailsDialog2,#senderDetailsDialog3,#senderDetailsDialog4,#senderDetailsDialog5,#senderDetailsDialog6,#senderDetailsDialog7,#senderDetailsDialog8,#senderDetailsDialog9").dialog({
                bgiframe: true,
                height: 300,
                width: 900,
                modal: true,
                autoOpen: false,
                resizable: false
            });
        }
        
        $('.showDialog').live('click', initDialog);
    
    </script>
    <script src='<%=Url.Content("~/Scripts/versionsBrowser.js") %>' type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="mainWrapper">
        <div id="document2ColumnWrapper">
        
    
    <h2></h2>
    
    <%=Html.ActionLink(Resources.Strings.S87, "Index", new { isBack = true }, new { @class="backToResult iconLink"})%>
    
    <%if (!Model.AllowDocumentDisplay)
      { %>
        
        <div class="infoMessage">
            <%= Html.Encode(Model.DenyMessage) %>
        </div>
      
    <%}
      else
      { %>
        <% if (AppContext.AllowInfoTypeOneWrite() ||
              AppContext.AllowInfoTypeTwoWrite() ||
               AppContext.AllowScansWrite())
           {  %>
                <%= Html.ActionLink(Resources.Strings.S88, "Edit", new { documentID = Model.Details.Document.DocumentID }, new { @class = "editDoc iconLink" })%>
        <%} %>
        
        <br /><br />
        <span>
        <i><%= Resources.Strings.S89 %>: <strong><%=Html.Encode(Membership.GetUser((object)Model.Details.Document.Author).UserName)%></strong></i>
        &nbsp&nbsp&nbsp
        <i><%= Resources.Strings.S84 %> <strong><%= Html.Encode(Model.Details.Document.CreateDate.ToShortDateString())%></strong></i>
        &nbsp&nbsp&nbsp
        <i>
            <%= Html.ActionLink(Resources.Strings.S90, "DocumentHistory", "Settings" , new {documentID = Model.Details.Document.DocumentID}, null) %>
        </i>
        </span>
        
        <%if (AppContext.AllowScansRead() && Model.Details.ScanIDs.Count > 0)
          {%>
            <fieldset>
            <ul class="imgPreview">
                <%foreach (Guid scanID in Model.Details.ScanIDs)
                  {%>
                      <li class="imgPreview">
                        <%= Html.Hidden("scanID", scanID)%>
                        <a class="verWrapper" href="#" ><%--onclick="setPreview('<%=Html.Encode(scanID.ToString()) %>')"--%>
                            <img src='<%=Url.Action("Preview", "Scans", new{scanGuid = scanID}) %>' alt='<%= Resources.Strings.S105 %>'/>
                            <%if (Model.HasVersions(scanID))
                              {%>
                                <img class="verIcon noBorder" src='<%=Url.Content("~/Content/icons/versions_icon.png") %>' alt='<%= Resources.Strings.S109 %>' />
                            <%}%>
                              
                        </a><br />
                        <a href="<%=Url.Action("Download", "Scans", new{scanGuid = scanID}) %>"><%= Resources.Strings.S21 %></a>
                        &nbsp;&nbsp;
                        <a href="<%=Url.Action("DownloadMarked", "Scans", new{scanGuid = scanID}) %>"><%= Resources.Strings.S123 %></a>
                        <br /><br />
                      </li>
                <%} %>
            </ul>
            </fieldset>
        <%} %>
        
        <fieldset class="">
            <legend><%= Resources.Strings.S91 %></legend>
            <ol>
             <%if (AppContext.AllowInfoTypeOneRead())
               {
                    InfoTypeOne infoTypeOne = Model.Details.InfoTypeOne;%>
                    
                    <% if (!string.IsNullOrEmpty(infoTypeOne.DocumentNumber)){ %>
                        <li><%= Html.LabeledPropertyValue(Resources.Strings.S29, infoTypeOne.DocumentNumber)%></li><%} %>
                    <% if (!string.IsNullOrEmpty(infoTypeOne.CaseNumber))
                       { %>
                        <li><%= Html.LabeledPropertyValue(Resources.Strings.S30, infoTypeOne.CaseNumber)%></li><%} %>
                    
                    <% if (infoTypeOne.Date.HasValue)
                       { %>
                        <li><%= Html.LabeledPropertyValue(Resources.Strings.S31, infoTypeOne.Date.Value.ToShortDateString())%></li><%} %>
                    
                    <% if (infoTypeOne.CategoryID.HasValue)
                       { %>
                        <br />
                        <li><%= Html.LabeledPropertyValue(Resources.Strings.S33, infoTypeOne.Category.Name)%></li><%} %>
                    
                    <% if (infoTypeOne.TypeID.HasValue)
                       { %>
                        <li><%= Html.LabeledPropertyValue(Resources.Strings.S34, infoTypeOne.Type.Name)%></li><%} %>
                        
                         <% if (!string.IsNullOrEmpty(infoTypeOne.Tags))
                       { %>
                        <li><%= Html.LabeledPropertyValue(Resources.Strings.S32, infoTypeOne.Tags)%></li><%} %>
                        
                         <% if (infoTypeOne.Type2ID.HasValue)
                       { %>
                        <li><%= Html.LabeledPropertyValue(Resources.Strings.S35, infoTypeOne.Type2.Name)%></li><%} %>
                    
            <% } %>
            <% if (AppContext.AllowInfoTypeTwoRead())
               {
                    InfoTypeTwo infoTypeTwo = Model.Details.InfoTypeTwo; %>
                    <% if (Model.Details.Senders.Count()>0)
                       { %>
                       
                        <li>
                            <label><%=Resources.Strings.S37%>: </label> 
                            <div class="inputLike">
                                <span>
                                <%
                                    int index = 0;
                           foreach(var sender in Model.Details.Senders){%> 
                             <a class="showDialog" title="szczegó³owe informacje" onclick="jQuery('#senderDetailsDialog<%=index.ToString()%>').dialog('open'); return false"> <%=Html.Encode(sender.FullName)%></a>,
                              <%index++;
                           } %>  
                                </span>
                            </div>
                        </li>
                        
                        
                    <% } %>
                    <% if (!string.IsNullOrEmpty(infoTypeTwo.Description))
                       { %>
                           
                            <li class="wide">
                                <label><%= Resources.Strings.S41 %>:</label>
                                <span class="description">
                                    <%= Html.Encode(infoTypeTwo.Description) %>
                                </span>
                            </li>
                    <% } %>
                    <% if(infoTypeTwo.Flag1) {%>
                    <li class="wide">
                    <label><%=Resources.Strings.S42%>: </label><span>Tak</span>
                    </li>
                    <%} %>
            <% } %>
            </ol>
        </fieldset>
        
        
    </div>
    <div id="preview2ColumnWrapper">
        <div class="previewCell">
            <%if (AppContext.AllowScansRead())
              {%>
                <div id="versionsBrowserBox">
                    
                    <% if (Model.Details.ScanIDs.Count > 0)
                        {
                            Html.RenderPartial("VersionBrowserUserControl", new eArchiver.Models.ViewModels.Documents.VersionBrowserViewModel(Model.Details.ScanIDs[0]));
                        } %>
                </div>
            <%} %>
        </div>
    </div>
</div>
        <%if (Model.Details.InfoTypeTwo != null /*&& Model.Details.InfoTypeTwo.Sender != null*/)
          { %>
          <% int j = 0;
              foreach( Sender sender in Model.Details.Senders){ %>
        <div id="senderDetailsDialog<%=j.ToString()%>" title='<%= Resources.Strings.S100 %>'>
            <fieldset class="twoColumn">
                <legend><%= Resources.Strings.S101 %></legend>
                <ol>
                    <li><%= Html.LabeledPropertyValue(Resources.Strings.S44, sender.FirstName)%></li>
                    <li><%= Html.LabeledPropertyValue(Resources.Strings.S45, sender.LastName)%></li>
                    <li><%= Html.LabeledPropertyValue(Resources.Strings.S46, sender.Company)%></li>
                    <li><%= Html.LabeledPropertyValue(Resources.Strings.S47, sender.Position)%></li>
                 
                    <li><%= Html.LabeledPropertyValue(Resources.Strings.S49, sender.Email)%></li>
                    <li><%= Html.LabeledPropertyValue(Resources.Strings.S52, sender.Webpage)%></li>                
                    <li><%= Html.LabeledPropertyValue(Resources.Strings.S50, sender.PhoneHome)%></li>
                    <li><%= Html.LabeledPropertyValue(Resources.Strings.S53, sender.PhoneMobile)%></li>
                    <li><%= Html.LabeledPropertyValue(Resources.Strings.S51, sender.PhoneWork)%></li>
                    <li><%= Html.LabeledPropertyValue(Resources.Strings.S54, sender.FaxWork)%></li>
                   
                    <li><%= Html.LabeledPropertyValue(Resources.Strings.S55, string.Format("{0}{1}{2} {3} {4}{5}{6}",
                        sender.PostCode
                        ,string.IsNullOrEmpty(sender.City)?"":", "
                        ,sender.City
                        ,sender.Street
                        ,sender.Building
                       ,(!string.IsNullOrEmpty(sender.Building))&&(!string.IsNullOrEmpty(sender.Flat))?"/":""
						,sender.Flat))%></li>
                    <li><%= Html.LabeledPropertyValue(Resources.Strings.S63, sender.Notes)%></li>
                </ol>
            </fieldset>
        </div>
        <%
        j++;} %>
        <%}
      }%>
</asp:Content>
