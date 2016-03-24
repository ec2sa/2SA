<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<eArchiver.Models.Sender>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	2SA - <%= Resources.Strings.S43 %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   <% Html.RenderPartial("SettingsSubsectionLinks"); %> 
    <h2><%= Resources.Strings.S43 %></h2>
    <% using (Html.BeginForm())
       { %>
       <%= Html.Hidden("SenderID", Model.SenderID) %>
        
        <%= Html.ValidationSummary() %>
        
        <fieldset class="twoColumn">
            <legend>Dane podstawowe</legend>
            <ol>
                <li><%=Html.LabeledTextBox("FirstName", Resources.Strings.S44, Model.FirstName)%></li>
                <li><%=Html.LabeledTextBox("LastName", Resources.Strings.S45, Model.LastName)%></li>
                <li><%=Html.LabeledTextBox("Company", Resources.Strings.S46, Model.Company)%></li>
                <li><%=Html.LabeledTextBox("Position", Resources.Strings.S47, Model.Position)%></li>
            </ol>
        </fieldset>
        <fieldset class="twoColumn">
            <legend><%= Resources.Strings.S48 %></legend>
            <ol>
                <li><%=Html.LabeledTextBox("Email", Resources.Strings.S49, Model.Email)%></li>
                <li><%=Html.LabeledTextBox("Webpage", Resources.Strings.S52, Model.Webpage)%></li>
                
                <li><%= Html.LabeledTextBox("PhoneHome", Resources.Strings.S50, Model.PhoneHome)%></li>
                <li><%= Html.LabeledTextBox("PhoneMobile",Resources.Strings.S53, Model.PhoneMobile)%></li>
                <li><%= Html.LabeledTextBox("PhoneWork", Resources.Strings.S51, Model.PhoneWork)%></li>
                <li><%= Html.LabeledTextBox("FaxWork", Resources.Strings.S54, Model.FaxWork)%></li>
            </ol>
        </fieldset>
        <fieldset class="twoColumn">
            <legend><%= Resources.Strings.S55 %></legend>
            <ol>
             <li>
                    <%= Html.LabeledTextBox("Street", Resources.Strings.S56, Model.Street)%>
                </li>
                <li>
                    <%= Html.LabeledTextBox("PostCode", Resources.Strings.S59, Model.PostCode)%>
                </li>
                 <li>
                    <%= Html.LabeledTextBox("Building", Resources.Strings.S57, Model.Building)%>
                </li>
                <li>
                    <%= Html.LabeledTextBox("City", Resources.Strings.S60, Model.City)%>
                </li>
               
               
                <li>
                    <%= Html.LabeledTextBox("Flat", Resources.Strings.S58, Model.Flat)%>
                </li>
                <li>
                    <%= Html.LabeledTextBox("Post", Resources.Strings.S61, Model.Post)%>
                </li>
               
            </ol>
        </fieldset>
      <fieldset><legend><%= Resources.Strings.S62 %></legend>
        <ol>
                <li>
                    <%= Html.LabeledTextArea("Notes", Resources.Strings.S63, 2, 35, Model.Notes)%>
                </li></ol></fieldset>
        <fieldset class="submit">
            <input type="submit" id="submit1" name="Submit" value='<%= Resources.Strings.S64 %>' />
            <input type="submit" id="submit2" name="Submit" value='<%= Resources.Strings.S65 %>'  />
       </fieldset>
    <%} %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsContent" runat="server">
</asp:Content>
