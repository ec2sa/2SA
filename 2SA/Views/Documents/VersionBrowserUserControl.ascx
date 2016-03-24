<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<eArchiver.Models.ViewModels.Documents.VersionBrowserViewModel>" %>

<%@ Import Namespace="eArchiver.Helpers" %>

    <fieldset class="versionView">
        <legend><%= Resources.Strings.S110 %></legend>
        
        <% if(Model.HasVersions)
           { %>
                <h3><%= Resources.Strings.S115 %>:</h3>
                <ul>
                    <li>
                        <% if(Model.HasPrevious)
                           { %>
                                <a class="versions" id="prevVersion" onclick="ShowScanVersion('<% =Html.Encode(Model.PreviousScanID.ToString()) %>')"><%= Resources.Strings.S116 %></a>
                        <% } %>
                    </li>
                    <li>
                        <% if(Model.HasNext)
                           { %>
                                <a class="versions" id="nextVersion" onclick="ShowScanVersion('<% =Html.Encode(Model.NextScanID.ToString()) %>')"><%= Resources.Strings.S117 %></a> 
                        <% } %>
                    </li>
                    <li>
                        <a class="versions iconCaption" id="downloadVersion" href="<%=Url.Action("Download", "Scans", new{scanGuid = Model.Scan.ScanID}) %>"><%= Resources.Strings.S21 %></a>
                    </li>
                    <br />
                </ul>
        <% } %>
        
        <%--<% if(Model.HasVersions)
           { %>
                <table>
                    <tr>
                        <td colspan="2" >
                            <div class="padded">Przegl¹daj wersje:</div>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:50px">
                        <% if(Model.HasPrevious)
                           { %>
                            <a class="versions iconCaption" id="prevVersion" onclick="ShowScanVersion('<% =Html.Encode(Model.PreviousScanID.ToString()) %>')">Poprzednia</a>
                        <% } %>
                        </td>
                        <td style="width:50px">
                        <% if(Model.HasNext)
                           { %>
                            <a class="versions iconCaption" id="nextVersion" onclick="ShowScanVersion('<% =Html.Encode(Model.NextScanID.ToString()) %>')">Nastêpna</a> 
                        <% } %>
                        </td>
                        <td style="width:50px">
                            <a class="versions iconCaption" id="downloadVersion" href="<%=Url.Action("Download", "Scans", new{scanGuid = Model.Scan.ScanID}) %>">Pobierz</a>
                        </td>
                    </tr>
                </table>
        <% } %>--%>
        <div>
            <img id="selectedScanPreview" src='<%= Url.Content(string.Concat("~/Scans/Zoom?scanGuid=", Model.Scan.ScanID)) %>' alt="Podgl¹d <%=Html.Headers().Skanu %>" />
        </div>
    </fieldset>