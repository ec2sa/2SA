<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<eArchiver.Models.ViewModels.Scans.ScanPreviewViewModel>" %>
<%@ Import Namespace="eArchiver.Helpers" %>

    <fieldset>
        <legend><%= Resources.Strings.S66 %></legend>
        <div>
            <% ScanTypeInfo scanType = ScansHelper.GetTypeInfo(Model.Scan.MimeType);
                if (scanType.IsImage || scanType.IsPdfDocument)
               { %>
                    <table>
                        <tr>
                            <td><a class="rotation" id="rotateLeft" /></td>
                            <td><a class="rotation" id="rotateRight" /></td> 
                            
                            <td><a class="rotation" id="saveRotation" /></td>
                            <td><a class="rotation" id="cancelRotation" /></td>
                        </tr>
                    </table>
            <%} %>
            <div>
                <img id="selectedScanPreview" src='<%= Url.Content(string.Concat("~/Scans/Zoom?scanGuid=", Model.Scan.ScanID, "&rotation=", Model.Rotation, "&rand=", Model.Random())) %>' alt='<%= Resources.Strings.S110 %>' />
            </div>
        </div>
    </fieldset>