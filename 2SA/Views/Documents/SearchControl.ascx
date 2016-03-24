<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<eArchiver.Models.ViewModels.Documents.DocumentSearchViewModel>" %>
<%@ Import Namespace="eArchiver.Controllers" %>
<%@ Import Namespace="eArchiver.Helpers" %>

<div>
    <%using (Html.BeginForm(new { p = 0 }))
      { %>
    <div id="accordion">
        <h4><a href="#"><%= Resources.Strings.S77 %></a></h4>
        <div>
            <fieldset class="narrow" style="width:99%">
            <%=Html.Hidden("SortType", Model.SortCriteria.SortType)%>
            <%=Html.Hidden("SortDirection", Model.SortCriteria.SortDirection)%>
            <table class="layoutOnly">
                <tr>
                 <% if (AppContext.AllowInfoTypeOneRead())
                    { %>
                    <td>
                        <label><%= Resources.Strings.S78 %></label>
                        <% if (Model.SearchCriteria.CreateDate.HasValue && Model.SearchCriteria.CreateDate.Value != DateTime.MinValue)
                           {%>
                        <%= Html.TextBox("CreateDate", Model.SearchCriteria.CreateDate.Value)%>                    
                        <%  }
                           else
                           {%>
                        <%= Html.TextBox("CreateDate")%>
                        <% } %>
                        -
                         <% if (Model.SearchCriteria.CreateDateTo.HasValue && Model.SearchCriteria.CreateDateTo.Value != DateTime.MinValue)
                            {%>
                        <%= Html.TextBox("CreateDateTo", Model.SearchCriteria.CreateDateTo.Value)%>                    
                        <%  }
                            else
                            {%>
                        <%= Html.TextBox("CreateDateTo")%>
                        <% } %>
                        <br />
                        
                         <label><%= Resources.Strings.S79 %></label>
                         <% if (Model.SearchCriteria.Date.HasValue && Model.SearchCriteria.Date.Value != DateTime.MinValue)
                            {%>
                        <%= Html.TextBox("Date", Model.SearchCriteria.Date.Value)%>                    
                        <%  }
                            else
                            {%>
                        <%= Html.TextBox("Date")%>
                        <% } %>
                        -
                         <% if (Model.SearchCriteria.DateTo.HasValue && Model.SearchCriteria.DateTo.Value != DateTime.MinValue)
                            {%>
                        <%= Html.TextBox("DateTo", Model.SearchCriteria.DateTo.Value)%>                    
                        <%  }
                            else
                            {%>
                        <%= Html.TextBox("DateTo")%>
                        <% } %>
                        <br />
                         <%= Html.LabeledTextBox("DocumentNumber", Resources.Strings.S29, Model.SearchCriteria.DocumentNumber)%>
                        <br />
                        <%= Html.LabeledTextBox("CaseNumber", Resources.Strings.S30, Model.SearchCriteria.CaseNumber)%>
                        <br />
                         <%= Html.LabeledTextBox("Tags", Resources.Strings.S32, Model.SearchCriteria.Tags)%>
                         <br />
                         <label for="Type2ID">
            <%=Resources.Strings.S35%>:</label>
            <%= Html.DropDownList("Type2ID", new SelectList(Model.Types2, "Type2ID", "Name", Model.SearchCriteria.Type2ID), Resources.Strings.S120)%>
            <br />
          

            <label for="CategoryID">
            <%=Resources.Strings.S33%>:</label>
            <%= Html.DropDownList("CategoryID", new SelectList(Model.Categories, "CategoryID", "Name", Model.SearchCriteria.CategoryID), Resources.Strings.S120)%>
            <br />
            <label for="TypeID">
            <%=Resources.Strings.S34%>:</label>
            <% if (Model.SearchCriteria.CategoryID.HasValue)
               { %>
            <%= Html.DropDownList("TypeID", new SelectList(Model.Types, "TypeID", "Name", Model.SearchCriteria.TypeID), Resources.Strings.S120)%><%}
               else
               { %>
            <select id="TypeID" name="TypeID">
            <option value=""><%=Resources.Strings.S120%></option>
            </select>
            <% } %>
                    </td>
                    <%} %>
                    
                    <td>
                       <% if (AppContext.AllowInfoTypeTwoRead())
                          { %>
            <div id="senderSelect">
                <%= Html.LabeledDropDownList("SenderID", Resources.Strings.S120, new SelectList(Model.Senders, "SenderID", "FullName", Model.SearchCriteria.SenderID), Resources.Strings.S37)%>
                <div>
                    <input type="text" id="senderSearch" class="searchBck" />
                </div>
            </div>
            <br />
            <%=Html.LabeledTextBox("Company", Resources.Strings.S46)%>
            <br />
            <%= Html.LabeledTextArea("Description", Resources.Strings.S41, Model.SearchCriteria.Description, 2, 45, null)%>
            <br />
            <% } %>
             <% if (AppContext.AllowContent())
                { %>
            <%= Html.LabeledTextArea("Text", Resources.Strings.S80, 2, 45, null)%>
            <br />
            <%= Html.SearchOcrCheckBox("SearchOCR", "Przeszukuj OCR", AppContext.AllowOCR())%>
            <br />
            <% } %>
                    </td>
                </tr>
                <tr>
                <td colspan="2">
                <br /><br />
                <label for="ItemsCount"><%= Resources.Strings.S81 %>:</label>
            <%=Html.DropDownList("ItemsCount", new SelectList(new int[] { 5, 10, 25, 50, 75, 100 }, Model.SearchCriteria.ItemsCount))%>
            <br />
             <input id="submitBtn" type="submit" value='<%= Resources.Strings.S82 %>' />
           
                </td>
                </tr>
            </table>
        
           
            </fieldset>
          
            

        </div>
    </div>
    
    <%if (Model.IsSearchPerformed)
      {
          if (Model.SearchResults.Count > 0)
          { %>
    <fieldset><legend><%= Resources.Strings.S83 %></legend>
    <div class="pager">
        <% if (Model.SearchResults.HasPreviousPage)
           { %><a href="#" onclick="changePage('<%= Model.SearchResults.PageIndex - 1%>')">&laquo</a><% } %>
        <span>
            <%= Html.Encode(string.Concat(Resources.Strings.S86 + " [",Model.SearchResults.CurrentPage, "] / [", Model.SearchResults.TotalPages,"]"))%>
        </span>
        <% if (Model.SearchResults.HasNextPage)
           {  %><a href="#" onclick="changePage('<%= Model.SearchResults.PageIndex + 1%>')">&raquo</a><% } %>
    </div>
    <table class="grid fullWidth">
        <tr>
            <th></th>
            <th id="hCreateDate"><a href="#" onclick="sortDocument('CreateDate')"><%= Resources.Strings.S84 %></a>
            </th>
            <% if (AppContext.AllowInfoTypeOneRead())
               { %>
            <th id="hDocNo"><a href="#" onclick="sortDocument('DocumentNumber')">
                <%=Resources.Strings.S29%></a></th>
            <th id="hCaseNo"><a href="#" onclick="sortDocument('CaseNumber')">
                <%=Resources.Strings.S30%></a></th>
            <th id="hDate"><a href="#" onclick="sortDocument('Date')">
                <%=Resources.Strings.S31%></a></th>
            <th id="hCategory"><a href="#" onclick="sortDocument('Category')">
                <%=Resources.Strings.S33%></a></th>
            <th id="hType"><a href="#" onclick="sortDocument('Type')">
                <%=Resources.Strings.S34%></a></th>
            <%} %>
            <% if (AppContext.AllowInfoTypeTwoRead())
               { %>
            <th id="hSender"><a href="#" onclick="sortDocument('Sender')">
                <%=Resources.Strings.S37%></a></th>
            <th id="hDesc"><a href="#" onclick="sortDocument('Description')">
                <%=Resources.Strings.S36%></a></th>
            <%} %>
            <% if (!string.IsNullOrEmpty(Model.SearchCriteria.Text) && AppContext.AllowOCR())
               { %>
            <th><%= Resources.Strings.S111 %></th>
            <% } %>
            <% if (AppContext.AllowInfoTypeOneRead() || AppContext.AllowInfoTypeTwoRead())
               {%>
            <th>&nbsp;</th>
            <% } %>
        </tr>
        <% foreach (var result in Model.SearchResults)
           {%>
        <tr>
            <td>
                <%if (result.ScanIDs != null && result.ScanIDs.Count > 0)
                  { %>
                <a target="_blank" href="<%= Url.Action("DocumentScans", new {d=result.Document.DocumentID, s=0})%>">
                    <img class="noBorder" src='<%=Url.Content("~/Content/icons/attachment.png")%>' />
                </a>
                <%} %>
            </td>
            <td>
                <%=Html.Encode(result.Document.CreateDate.ToShortDateString())%>
            </td>
            <% if (AppContext.AllowInfoTypeOneRead())
               { %>
            <td>
                <%=Html.Encode(result.InfoTypeOne.DocumentNumber)%>
            </td>
            <td>
                <%=Html.Encode(result.InfoTypeOne.CaseNumber)%>
            </td>
            <td>
                <% if (result.InfoTypeOne.Date.HasValue)
                   {%>
                <%=Html.Encode(result.InfoTypeOne.Date.Value.ToShortDateString())%>
                <% } %>
            </td>
            <td>
                <% string category = string.Empty;
                   if (result.InfoTypeOne.Category != null)
                       category = result.InfoTypeOne.Category.Name;  %>
                <%=Html.Encode(category)%>
            </td>
            <td>
                <% string type = string.Empty;
                   if (result.InfoTypeOne.Type != null)
                       type = result.InfoTypeOne.Type.Name;  %>
                <%=Html.Encode(type)%>
            </td>
            <% } %>
            <% if (AppContext.AllowInfoTypeTwoRead())
               { %>
            <td>
                <%=Html.Encode(result.DocumentSenders)%>
            </td>
            <td>
                <%if (result.InfoTypeTwo.Description != null)
                  {
                      string desc = result.InfoTypeTwo.Description;
                      if (desc.Length > 50)
                          desc = string.Concat(desc.Substring(0, 47), "..."); %>
                <%= Html.Encode(desc)%>
                <%} %>
            </td>
            <% } %>
            <% if (!string.IsNullOrEmpty(Model.SearchCriteria.Text) && AppContext.AllowOCR())
               { %>
            <td>
                <%= Html.Encode(result.ScanContentInfo) %>
            </td>
            <% } %>
            <% if (AppContext.AllowInfoTypeOneRead() || AppContext.AllowInfoTypeTwoRead())
               {%>
            <td>
                <%= Html.ActionLink(Resources.Strings.S85, "Details", new { documentID = result.Document.DocumentID })%>
            </td>
            <% } %>
        </tr>
        <% }%>
    </table>
    </fieldset>
    <% }
          else
          { %>
    <div class="infoMessage">
        <%= Resources.Strings.S103 %>.
    </div>
    <% } %>
    <%} %>
    
    <% } %>
</div>