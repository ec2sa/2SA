<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<eArchiver.Models.ViewModels.Documents.DocumentEditViewModel>" %>

<%@ Import Namespace="eArchiver.Controllers" %>
<%@ Import Namespace="eArchiver.Models" %>
<%@ Import Namespace="eArchiver.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    2SA - Edycja
    <%=Resources.Strings.S29 %>
</asp:Content>
<asp:Content ID="ScriptsCont" ContentPlaceHolderID="ScriptsContent" runat="server">

    <script type="text/javascript">
    <!--
        $(function() {
        $("#sendersIDs").val("<%=Model.Details.SenderIDs%>");
        if ($("#sendersIDs").val() == "")
            $("#sendersList").html("");
        
            $("#submitBtn").click(function() {
                $("#selectedScans").val("");
                $("ul.imgPreview > li").each(
                    function() {
                        var sid = $('input[type="hidden"]:first', $(this)).val();
                        var scans = $("#selectedScans").val();
                        $("#selectedScans").val(scans + "," + sid);
                    });
                return true;
            });

            $('#senderSearch2').focus(function() { $(this).val(''); });
            $("#senderSearch2").autocomplete({
                source: '<%=Url.Action("SenderSearch","Scans") %>',
                minLength: 3,
                select: function(event, ui) {
                    if (ui.item) {

                        $('#SenderID').val(ui.item.id);
                        $('#SenderID').focus();
                    }
                    else
                        $('#SenderID').val('');
                }
            });

        });

        function initDialog() {
            $("#senderDialog").dialog({
                bgiframe: true,
                height: 430,
                width: 900,
                modal: true,
                autoOpen: false,
                resizable: false
            });
            $("#scansDialog").dialog({
                bgiframe: true,
                height: 550,
                width: 920,
                modal: true,
                autoOpen: false,
                resizable: false
            });
            $("#versionsDialog").dialog({
                bgiframe: true,
                height: 120,
                width: 350,
                modal: true,
                autoOpen: false,
                resizable: false
            });
            $("#uploadScanDialog").dialog({
                bgiframe: true,
                height: 120,
                width: 350,
                modal: true,
                autoOpen: false,
                resizable: false
            });
        }

        $(document).ready(function() {
            initDialog();

            $(".deleteLink").click(function() {
                var delBtn = $(this);
                var scanGuid = $('input[type="hidden"]:first', delBtn.parent()).val();

                var div = $(document.createElement("div"));
                div.toggleClass("scansGridItem");
                div.append($('input[type="hidden"]:first', delBtn.parent()));
                div.append($("img", delBtn.parent()).clone());

                var link = $("a.preview", delBtn.parent()).clone();
                link.toggleClass("preview");
                link.html("Pobierz");
                var content = $("a:eq(2)", delBtn.parent()).attr("href");
                link.attr("href", content)
                div.append(link);

                div.append($("<br />"));
                div.append($("<a href='#' class='scanSelector'><%= Resources.Strings.S39 %></a>"));

                $("a:last", div).bind("click", function() {
                    addScanFromGrid($(this));
                });

                var item;
                if ($("table tr:last").children().size() < 3) {
                    item = $(document.createElement("td"));
                    item.html(div);
                    $("table#scansGrid tr:last").append(item);
                }
                else {
                    item = $(document.createElement("tr"));
                    item.html(document.createElement("td"));
                    $("td", item).html(div);
                    $("table#scansGrid tbody").append(item);
                }
                $("#li_" + scanGuid).fadeOut(500, function() { $(this).remove(); });
            });

            $("#Date").datepicker({ dateFormat: 'yy-mm-dd' });
            $("#Date").blur(function() {
                isDateValid(this);
            });
            showHideDialog();

        });

        $('.showDialog').live('click', initDialog);

        function showHideDialog() {
            if ($.getAllQueryStrings()["d"] != null && $.getAllQueryStrings()["d"].value == "1") {
                jQuery('#scansDialog').dialog('open');
            }
            else {
                //jQuery('#scansDialog').dialog('destroy');
            }
        }

        function setVersionGuid(guid) {
            $("#versionsDialog input[type='hidden']").val(guid);
        }

        function setListVersionGuid(guid) {

            $("#scansDialog #originalScanID").val(guid);
        }

        function clearVersionGuid() {
            $("#scansDialog #originalScanID").val("");
        }

        function setPreview(scanGuid) {
            $('#selectedScanPreview').attr('src', '/Scans/Zoom?scanGuid=' + scanGuid);
        }

        function addSender(senderDropdown, sendersList, sendersIDs) {
            var sdd = $("#" + senderDropdown);
            var slist = $("#" + sendersList);
            var selectedSender = $('option:selected', sdd);
            var sids = $("#" + sendersIDs);

            if (!sdd || !selectedSender || selectedSender.val() == '' || !slist || !sids) {

                return;
            }
            slist.append(selectedSender);
            generateSIDs(sids, slist)


        }

        function removeSender(senderDropdown, sendersList, sendersIDs) {
            var sdd = $("#" + senderDropdown);
            var slist = $("#" + sendersList);
            var selectedSender = $('option:selected', slist);
            var sids = $("#" + sendersIDs);

            if (!sdd || !slist || !sids || !selectedSender) {

                return;
            }
            sdd.append(selectedSender);
            generateSIDs(sids, slist);


        }

        function generateSIDs(sids, slist) {
            temp = '';
            $("option", slist).each(function(index, opt) {
                if (temp.length > 0)
                    temp += ',';
                temp += opt.value;
                sids.val(temp);

            });

        }
 -->
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
    <%--Edytuj <%=Html.Headers().Dokument%>--%></h2>
    <%if (!Model.AllowDocumentDisplay)
      { %>
    <div class="infoMessage">
        <%= Html.Encode(Model.DenyMessage) %>
    </div>
    <%}
      else
      { %>
    <%= Html.Hidden("currentDocumentId", Model.Details.Document.DocumentID) %>
    <span><i><%= Resources.Strings.S89 %>: <b>
        <%=Html.Encode(Membership.GetUser((object)Model.Details.Document.Author).UserName) %></b></i>
        &nbsp;&nbsp;&nbsp; <i><%= Resources.Strings.S84 %>: <b>
            <%= Html.Encode(Model.Details.Document.CreateDate.ToShortDateString()) %></b></i>
    </span>
    <br />
    <br />
    <% using (Html.BeginForm())
       { %>
    <%if (AppContext.AllowScansWrite())
      {%>
    <div>
        <a class="showDialog" href="#" onclick="clearVersionGuid();jQuery('#scansDialog').dialog('open'); return false;">
            <%= Resources.Strings.S92 %></a> &nbsp;&nbsp;|&nbsp;&nbsp; <a class="showDialog" href="#" onclick="jQuery('#uploadScanDialog').dialog('open'); return false;">
                <%= Resources.Strings.S93 %></a>
        <div id="scansDialog" title='<%= Resources.Strings.S96 %>'>
            <% Html.RenderPartial("../Scans/ScanSelectorUserControl", Model.ScanSelectorModel); %>
        </div>
    </div>
    <fieldset>
    <input type="hidden" id="selectedScans" name="selectedScans" />
    

    <ul class="imgPreview">
 
    <%foreach (Guid scanID in Model.Details.ScanIDs)
      {
          string itemId = string.Concat("li_", scanID);
    %>
    <li class="imgPreview" id='<%= Html.Encode(itemId) %>'>
        <div class="editPreview">
            <input type="hidden" value='<%= Html.Encode(scanID) %>' />
            <a href="#">
                <img src='<%=Url.Action("Preview", "Scans", new { scanGuid = scanID }) %>' alt='<%= Resources.Strings.S105 %>' />
            </a><a href="#" class="deleteLink">x</a>
            <br />
            <a href="<%=Url.Action("Download", "Scans", new { scanGuid = scanID }) %>"><%= Resources.Strings.S21 %></a>
            <br />
            <br />
            <a class="showDialog" href="#" onclick="setListVersionGuid('<%= Html.Encode(scanID) %>');jQuery('#scansDialog').dialog('open'); return false">
                <%= Resources.Strings.S94 %></a>
            <br />
            <a href="#" onclick="setVersionGuid('<%= Html.Encode(scanID) %>');jQuery('#versionsDialog').dialog('open'); return false">
                <%= Resources.Strings.S95 %></a>
        </div>
    </li>
    <%} %>
    </ul>
    </fieldset>
    <%} %>
    <% if (AppContext.AllowInfoTypeOneWrite())
       {
           InfoTypeOne infoTypeOne = Model.Details.InfoTypeOne;
    %>
    <fieldset class="narrow"><legend><%= Resources.Strings.S28 %></legend>
    <table class="layoutOnly">
        <tr>
            <td style="padding-right: 2em;">
                <%= Html.LabeledTextBox("DocumentNumber", Resources.Strings.S29, infoTypeOne.DocumentNumber)%>
                <br />
                <%= Html.LabeledTextBox("CaseNumber", Resources.Strings.S30, infoTypeOne.CaseNumber)%>
                <br />
                <% if (infoTypeOne.Date.HasValue)
                   { %>
                <%= Html.LabeledDatePicker("Date", Resources.Strings.S31, infoTypeOne.Date.Value)%><%}
                   else
                   { %>
                <%= Html.LabeledDatePicker("Date", Resources.Strings.S31)%><%} %>
                <br />
                <%= Html.LabeledTextBox("Tags", Resources.Strings.S32, infoTypeOne.Tags)%>
            </td>
            <td>
                <%if (infoTypeOne.Type2ID.HasValue)
                  { %>
                <%= Html.LabeledDropDownList("Type2ID", Resources.Strings.S120, new SelectList(Model.Types2, "Type2ID", "Name", infoTypeOne.Type2ID.Value), Resources.Strings.S35)%>
                <%}
                  else
                  {%>
                <%= Html.LabeledDropDownList("Type2ID", Resources.Strings.S120, new SelectList(Model.Types2, "Type2ID", "Name"), Resources.Strings.S35)%>
                <%} %>
                <br />
                <% if (infoTypeOne.CategoryID.HasValue)
                   {%>
                <%= Html.LabeledDropDownList("CategoryID", Resources.Strings.S120, new SelectList(Model.Categories, "CategoryID", "Name", infoTypeOne.CategoryID.Value), Resources.Strings.S33)%>
                <% if (infoTypeOne.TypeID.HasValue)
                   { %>
                <%= Html.LabeledDropDownList("TypeID", Resources.Strings.S120, new SelectList(Model.Types, "TypeID", "Name", infoTypeOne.TypeID.Value), Resources.Strings.S34)%><% }
                   else
                   { %>
                <%= Html.LabeledDropDownList("TypeID", Resources.Strings.S120, new SelectList(Model.Types, "TypeID", "Name"), Resources.Strings.S34)%><%} %>
                <% }
                   else
                   { %>
                <%= Html.LabeledDropDownList("CategoryID", Resources.Strings.S120, new SelectList(Model.Categories, "CategoryID", "Name"), Resources.Strings.S33)%>
                <label for="TypeID">
                <%=Resources.Strings.S34%>:</label>
                <select id="TypeID" name="TypeID">
                <option value=""><%=Resources.Strings.S120%></option>
                </select>
                <% } %>
            </td>
        </tr>
    </table>
    </fieldset>
    <%} %>
    <% if (AppContext.AllowInfoTypeTwoWrite())
       {
           InfoTypeTwo infoTypeTwo = Model.Details.InfoTypeTwo;
    %>
    <fieldset class="narrow"><legend><%=Resources.Strings.S36%></legend>
    <input type="hidden" id="sendersIDs" name="sendersIDs" />
    <table class="layoutOnly">
        <tr>
            <td style="padding-right: 2em;">
                <label for="sendersList">
                <%= Resources.Strings.S37%></label>
                <select id="sendersList" size="4" style="width: 100%">
               <%if (Model.Details.Senders.Count == 0)
                 { %>
                  <option>?</option>
               <%} else 
                foreach (var sender in Model.Details.Senders)
                      { %>
                      <option value="<%=sender.SenderID.ToString() %>"><%=sender.FullName %></option>
                    <%} %>
                </select><br />
                <input type="button" value="Usuñ" title="Usuñ z listy" onclick="removeSender('SenderID','sendersList','sendersIDs')" />
                <div id="senderDialog" title="<%= Resources.Strings.S43 %>">
                    <% Html.RenderPartial("SenderUserControl"); %>
                </div>
                <br />
                <br />
                <%= Html.LabeledTextArea("Description", Resources.Strings.S41, infoTypeTwo.Description, 4, 50, null)%>
                <br />
                <label><%=Html.CheckBox("Flag1",infoTypeTwo.Flag1)%> <%=Resources.Strings.S42%></label>
            </td>
            <td>
                <div id="senderSelect">
                    <a class="showDialog" href="#" onclick="jQuery('#senderDialog').dialog('open'); return false">
                        <img alt="ikonka- nowy nadawca" title='<%= Resources.Strings.S43 %>' class="noBorder" src='<%= Url.Content("~/Content/icons/new_sender_small.png") %>' />
                    </a>
                    <%= Html.DropDownList("SenderID", new SelectList(Model.FilteredSenders, "SenderID", "FullName"), Resources.Strings.S120)%>
                    <input type="button" value='<%= Resources.Strings.S39 %>' title="Dodaj do listy" onclick="addSender('SenderID','sendersList','sendersIDs')" />
                    <div>
                        <input type="text" id="senderSearch2" />
                    </div>
                    <br />
                </div>
            </td>
        </tr>
    </table>
    </fieldset>
    <% } %>
<div>
    <input id="submitBtn" name="Submit" type="submit" value='<%= Resources.Strings.S64 %>' />
    <input id="submitBtn2" name="Submit" type="submit" value='<%= Resources.Strings.S65 %>' />
</div>
    <%}%>
    <div id="versionsDialog" title="Dodaj wersjê">
        <form id="ajaxUploadVersion" action='<%= Url.Action("UploadScanVersion", "Documents", new {documentID = Model.Details.Document.DocumentID})%>'
        method="post" enctype="multipart/form-data">
        <div>
        <input type="hidden" id="originalID" name="originalID" />
        <%--<input type="hidden" id="documentID" name="documentID" value='<%=Html.Encode(Model.Details.Document.DocumentID) %>' />--%>
        <input type="file" name="file" />
        <input id="btnUploadVersion" type="submit" value="Dodaj" />
        </div>
        </form>
    </div>
    <div id="uploadScanDialog" title="Dodaj skan">
        <form id="ajaxUploadScan" action='<%= Url.Action("UploadScan", "Documents", new {documentID = Model.Details.Document.DocumentID})%>'
        method="post" enctype="multipart/form-data">
        <div>
        <%--<input type="hidden" id="documentID" name="documentID" value='<%=Html.Encode(Model.Details.Document.DocumentID) %>' />--%>
        <input type="file" name="file" />
        <input id="btnUploadScan" type="submit" value="Dodaj" />
        </div>
        </form>
    </div>
    <%} %>
</asp:Content>
