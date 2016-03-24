<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<eArchiver.Models.ViewModels.Scans.ScansViewModel>" %>

<%@ Import Namespace="eArchiver.Controllers" %>
<%@ Import Namespace="eArchiver.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    2SA -
    <%=Resources.Strings.S16 %>
</asp:Content>
<asp:Content ID="ScriptsCont" ContentPlaceHolderID="ScriptsContent" runat="server">
    <script src='<%=Url.Content("~/Scripts/rotation.js") %>' type="text/javascript"></script>
    <script type="text/javascript">
    
    <!--
   
    
        $(document).ready(function() {
        $('#senderSearch2').focus(function(){$(this).val(''); });
        $( "#senderSearch2" ).autocomplete({
			source: '<%=Url.Action("SenderSearch","Scans") %>',
			minLength: 3,
			select: function( event, ui ) {
			if(ui.item){
			
			$('#SenderID').val(ui.item.id);
			$('#SenderID').focus();
			}
			else
			$('#SenderID').val('');
			}
		});

        
        $("#sendersList").html("");
            initDialog();
            InitStack();
            $(".scanSelector").click(function() {
                var sl = $(this);
                sl.parent().toggleClass("selectedScan");
                var scanGuid = $('input[type="hidden"]:first', sl.parent()).val();
                if (sl.html() == 'Zaznacz') {
                    Push(scanGuid);
                    ResetRotation();
                    ShowScan(scanGuid);
                    sl.html('<%= Resources.Strings.S27 %>');
                }
                else {
                    Remove(scanGuid);
                    ResetRotation();
                    ShowScan(Pop());
                    sl.html('<%= Resources.Strings.S26 %>');
                }
                return false;
            });

            $(".scanSelectorImg").click(function() {
                var sl = $(this);
                sl.parent().toggleClass("selectedScan");
                var cssclass = sl.parent().attr("class");
                var scanGuid = $('input[type="hidden"]:first', sl.parent()).val();
                
                if (cssclass.indexOf("selectedScan") != -1) {
                    Push(scanGuid);
                    ResetRotation();
                    ShowScan(scanGuid);
                    $(".scanSelector", sl.parent()).html('<%= Resources.Strings.S27 %>');
                }
                else {
                    Remove(scanGuid);
                    ResetRotation();
                    ShowScan(Pop());
                    $(".scanSelector", sl.parent()).html('<%= Resources.Strings.S26 %>');
                }
                return false;
            });

            $(".deleteScan").click(function() {
                var delLink = $(this);
                var scanGuid = $('input[type="hidden"]:first', delLink.parent()).val();
                $.getJSON("<%=Url.Action("AddScanToRecycleBin","Custom") %>?scanID=" + scanGuid, function(data) {
                    if (data == true) {
                        Remove(scanGuid);
                        ShowScan(Pop());
                        delLink.parent().fadeOut("slow");
                    }
                });
            });

            $("#Date").datepicker({dateFormat: 'yy-mm-dd'});
             $("#Date").blur(function(){
            isDateValid(this);
             });
            
//            $("#senderSearch").keypress(function(e) {
//                $('#SenderID option[selected="selected"]:first').removeAttr("selected");
//                var text = $("#senderSearch").val();             
//                    text = text + String.fromCharCode(e.which);
//                $('#SenderID option:Contains(' + text + '):first').attr("selected", "selected");
//            });
            
            $('#getScansBtn').live("click", function(){
                var action = rootPath + "/Scans/GetScansFromFolder";
                $('#loading').show();
                $.getJSON(action, null, function(result) 
                {
                    if(result == true){
                        $('#loading').hide();
                        window.location.reload();
                    }
                });
            });
        });
        
        $(function() {
            $("#submitBtn").click(function() {
            
                $(".selectedScan").each(
                function() {
                    var sid = $('input[type="hidden"]:first', $(this)).val();
                    var scans = $("#selectedScans").val();
                    $("#selectedScans").val(scans + "," + sid);
                });
                return true;
            });
            
//            $('#btnUploadScan').click(function(){
//                $('#uploadScanDialog').dialog('close');
//                $('#loading').show();
//                $('#ajaxUploadScan').submit();
//            });
            
        });

        function initDialog() {
            $("#senderDialog").dialog({
                bgiframe: true,
                height: 550,
                width: 900,
                modal: true,
                autoOpen: false,
                resizable: false
            });
            $("#uploadScanDialog").dialog({
                bgiframe:true,
                width:450,
                modal:true,
                autoOpen:false,
                resizable:false,
                buttons:{
                   '<%= Resources.Strings.S39 %>':function(){
                        $(this).dialog('close');
                        
                        if($(".MultiFile-label").size() > 0)
                        {
                            $('#loading').show();
                            $('#ajaxUploadScan').submit();
                        }
                    },
                    "Anuluj": function(){
                        $(this).dialog('close');
                    }
                }
            });
        }
        
        $('.showDialog').live('click', initDialog);
        
        function addSender(senderDropdown,sendersList,sendersIDs){
        var sdd=$("#"+senderDropdown);
        var slist=$("#"+sendersList);
        var selectedSender= $('option:selected',sdd);
        var sids=$("#"+sendersIDs);
       
        if(!sdd || !selectedSender || selectedSender.val()=='' || !slist || !sids){
        
        return;
        }
        slist.append(selectedSender);
       generateSIDs(sids,slist)
    
        
        }
        
         function removeSender(senderDropdown,sendersList,sendersIDs){
        var sdd=$("#"+senderDropdown);
        var slist=$("#"+sendersList);
        var selectedSender= $('option:selected',slist);
        var sids=$("#"+sendersIDs);
        
        if(!sdd ||  !slist || !sids ||!selectedSender){
   
        return;
        }
        sdd.append(selectedSender);
       generateSIDs(sids,slist);
        
        
        }
        
        function generateSIDs(sids,slist){
        temp='';
        $("option",slist).each(function(index, opt) {
     if(temp.length>0)
         temp+=',';
         temp+=opt.value;
         sids.val(temp);
});

}
-->
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
    </h2>
    <div id="loading" style="display: none;">
        <strong>
            <%= Resources.Strings.S112 %></strong>
    </div>
    <div id="mainWrapper">
        <% using (Html.BeginForm("NewDocument", "Documents"))
           { %>
        <%if (AppContext.AllowScansWrite())
          { %>
        <div id="scansColumnWrapper">
            <div class="scansInfo">
                <div class="padded">
                    <%= Resources.Strings.S20 %>
                    <%=string.Format("{0}/{1}", Model.KnownScansCount, Model.AllScansCount)%>&nbsp;&nbsp;
                    <%if (Model.AllScansCount > 0)
                      { %>
                    <a href="#" id="getScansBtn">
                        <%= Resources.Strings.S21 %></a>&nbsp;&nbsp;
                    <% } %>
                    <a href="" onclick="window.location.reload();return false;">
                        <%= Resources.Strings.S22 %></a>
                </div>
                <div class="padded">
                    <a href="#" onclick="jQuery('#uploadScanDialog').dialog('open'); return false">
                        <%= Resources.Strings.S23 %></a>&nbsp;&nbsp;
                    <%if (AppContext.IsAdmin())
                      { %><%= Html.ActionLink(Resources.Strings.S24, "ScansBin", "Scans")%><%} %>
                </div>
            </div>
            <div id="currentScans">
                <input type="hidden" id="selectedScans" name="selectedScans" />
                <input type="hidden" id="sendersIDs" name="sendersIDs" />
                <% foreach (var item in Model.AvailableScans)
                   {%>
                <div class="scanPreview">
                    <input type="hidden" value='<%=Html.Encode(item.ScanID) %>' />
                    <a href='' class="scanSelectorImg" title='<%=Html.Encode(item.FileName) %>'>
                        <img src=' <%=Url.Action("Preview",new{scanGuid=item.ScanID}) %>' alt='<%= Resources.Strings.S113 %> <%=Html.Encode(item.FileName) %>' />
                    </a>
                    <br />
                    <a href='' class="scanSelector" title='<%=Html.Encode(item.FileName) %>'>
                        <%= Resources.Strings.S26 %></a> <a href='<%=Url.Action("Download",new{scanGuid=item.ScanID}) %>'
                            title='<%=Html.Encode(item.FileName) %>'>
                            <%= Resources.Strings.S21 %></a> <a href="#" class="deleteScan">
                                <%= Resources.Strings.S25 %></a>
                    <%if (item.MimeType.EndsWith("pdf") && item.PageCount>1)
                      { %>
                    <a href='<%=Url.Action("SplitFile",new{scanGuid=item.ScanID}) %>' title='<%=Html.Encode(item.FileName) %>'>
                        <%= Resources.Strings.S124%></a>
                    <%} %>
                    <p>
                        <strong>
                            <%=Html.Encode(item.FileName)%></strong>
                    </p>
                    <p>
                        <%=Html.Encode(string.Format("{0:yyyy-MM-dd} {0:hh:mm}", item.ImportDate))%>
                    </p>
                </div>
                <%} %>
            </div>
        </div>
        <%} %>
        <div id="documentColumnWrapper">
            <%if (Model.DocumentID.HasValue)
              { %>
            <div class="infoMessage">
                <%=Html.ActionLink(Resources.Strings.S69, "Details", "Documents", new {documentID = Model.DocumentID} , null) %>
            </div>
            <%} %>
            <% if (AppContext.AllowInfoTypeOneWrite())
               { %>
            <fieldset class="narrow">
                <legend>
                    <%= Resources.Strings.S28 %></legend>
                <table class="layoutOnly">
                    <tr>
                        <td style="padding-right: 2em;">
                            <label for="DocumentNumber">
                                <%= Resources.Strings.S29 %>
                            </label>
                            <%= Html.TextBox("DocumentNumber") %>
                            <br />
                            <label for="CaseNumber">
                                <%= Resources.Strings.S30 %>
                            </label>
                            <%= Html.TextBox("CaseNumber")%>
                            <br />
                            <label for="Date">
                                <%= Resources.Strings.S31 %>
                            </label>
                            <%= Html.TextBox("Date")%>
                            <br />
                            <label for="Tags">
                                <%= Resources.Strings.S32 %>
                            </label>
                            <%= Html.TextBox("Tags")%>
                        </td>
                        <td>
                            <label for="CategoryID">
                                <%= Resources.Strings.S33 %>:</label>
                            <%= Html.DropDownList("CategoryID", new SelectList(Model.Categories, "CategoryID", "Name"), Resources.Strings.S120)%>
                            <br />
                            <label for="TypeID">
                                <%= Resources.Strings.S34 %>:</label>
                            <select id="TypeID" name="TypeID">
                                <option value="">
                                    <%=Resources.Strings.S120%></option>
                            </select>
                            <br />
                            <label for="Type2ID">
                                <%= Resources.Strings.S35 %>:</label>
                            <%= Html.DropDownList("Type2ID", new SelectList(Model.Types2, "Type2ID", "Name"), Resources.Strings.S120)%>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <%} %>
            <% if (AppContext.AllowInfoTypeTwoWrite())
               { %>
            <fieldset class="narrow">
                <legend>
                    <%= Resources.Strings.S36 %></legend>
                <table class="layoutOnly">
                    <tr>
                        <td style="padding-right: 2em;">
                            <label for="sendersList">
                                <%= Resources.Strings.S37 %></label>
                            <select id="sendersList" size="4" style="width: 100%">
                                <option>?</option>
                            </select><br />
                            <input type="button" value='<%= Resources.Strings.S40 %>' title="Usuñ z listy" onclick="removeSender('SenderID','sendersList','sendersIDs')" />
                            <div id="senderDialog" title="<%= Resources.Strings.S43 %>">
                                <% Html.RenderPartial("SenderUserControl"); %>
                            </div>
                            <br />
                            <br />
                            <label for="Description">
                                <%= Resources.Strings.S41 %></label>
                            <%= Html.TextArea("Description", null, 5, 50, null)%>
                            <br />
                            <label>
                                <%=Html.CheckBox("Flag1")%>
                                <%= Resources.Strings.S42 %>
                            </label>
                        </td>
                        <td>
                            <div id="senderSelect">
                                <label>
                                    <%= Resources.Strings.S38 %></label>
                                <a class="showDialog" href="#" onclick="jQuery('#senderDialog').dialog('open'); return false">
                                    <img alt="ikona - nowy nadawca" title="Nowy nadawca" class="noBorder" src='<%= Url.Content("~/Content/icons/new_sender_small.png") %>' />
                                </a>
                                <%= Html.DropDownList("SenderID", new SelectList(Model.Senders, "SenderID", "FullName"), Resources.Strings.S120)%>
                                <div>
                                    <input type="text" id="senderSearch2" />
                                </div>
                                <input type="button" value='<%= Resources.Strings.S39 %>' title="Dodaj do listy"
                                    onclick="addSender('SenderID','sendersList','sendersIDs')" />
                            </div>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <% } %>
            <% if (AppContext.AllowInfoTypeOneWrite() || AppContext.AllowInfoTypeTwoWrite() || AppContext.AllowScansWrite())
               { %>
            <%--<fieldset class="submit narrow">--%>
            <input id="submitBtn" type="submit" value='<%= Resources.Strings.S67 %>' />
            <input type="reset" value='<%= Resources.Strings.S68 %>' />
            <%--   </fieldset>--%>
            <% } %>
        </div>
        <%} %>
        <div id="previewColumnWrapper">
            <%if (AppContext.AllowScansRead())
              {%>
            <input type="hidden" id="visibleScan" />
            <div id="scanPreviewBox" />
            <%} %>
        </div>
        <div id="uploadScanDialog" title='<%= Resources.Strings.S70 %>'>
            <form id="ajaxUploadScan" action='<%= Url.Action("UploadScan", "Scans")%>' method="post"
            enctype="multipart/form-data">
            <div class="fileselectbox">
                <strong>
                    <%= Resources.Strings.S71 %>:</strong>
                <input id="fileUploadx" class="multi" type="file" name="file" />
                <%--<input id="btnUploadScan" type="button" value="Dodaj" />--%>
            </div>
            </form>
        </div>
    </div>
</asp:Content>
