<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<eArchiver.Models.ViewModels.Documents.DocumentScansViewModel>" %>
<%@ Import Namespace="eArchiver.Helpers" %>

<fieldset>
    <legend><%= Resources.Strings.S110 %></legend>
    <ul class="none">
        <%for (int i = 0; i < Model.Details.ScanIDs.Count; i++)
          {%>
              <%if (i == Model.CurrentScanIndex)
              {%>
                <li>
                    <strong><%= Html.Encode(i+1) %></strong>
                </li>          
            <%}
              else
              { %>
                <li>
                    <a href="<%= Url.Action("DocumentScans", new {d=Model.Details.Document.DocumentID, s=i}) %>" ><%= Html.Encode(i+1) %></a>
                </li>
          
        <%    }%>
        <% } %>
    </ul>

    <div>
        <div>
            <img id="selectedScanPreview" src='<%= Url.Content(string.Concat("~/Scans/Zoom?scanGuid=", Model.Details.ScanIDs[Model.CurrentScanIndex])) %>' alt='<%= Resources.Strings.S110 %>' />
        </div>
    </div>
</fieldset>