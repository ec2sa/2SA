<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	2SA - Nowy nadawca
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%= Resources.Strings.S43 %></h2>
    <%= Html.ValidationSummary() %>
    
    <%using (Html.BeginForm())
      { %>
        <fieldset class="twoColumn">
        <legend><%= Resources.Strings.S121 %></legend>
        <ol>
            <li><%=Html.LabeledTextBox("FirstName", Resources.Strings.S44)%></li>
            <li><%=Html.LabeledTextBox("LastName", Resources.Strings.S45)%></li>
            <li>
            <%=Html.LabeledTextBox("Company", Resources.Strings.S46)%>
         
            </li>
            <li><%=Html.LabeledTextBox("Position", Resources.Strings.S47)%></li>
        </ol>
    </fieldset>
    <fieldset class="twoColumn">
        <legend><%= Resources.Strings.S48 %></legend>
        <ol>
            <li><%=Html.LabeledTextBox("Email", Resources.Strings.S49)%></li>
            <li><%=Html.LabeledTextBox("Webpage", Resources.Strings.S52)%></li>
            
            <li><%= Html.LabeledTextBox("PhoneHome", Resources.Strings.S50)%></li>
            <li><%= Html.LabeledTextBox("PhoneMobile", Resources.Strings.S53)%></li>
            <li><%= Html.LabeledTextBox("PhoneWork", Resources.Strings.S51)%></li>
            <li><%= Html.LabeledTextBox("FaxWork", Resources.Strings.S54)%></li>
        </ol>
    </fieldset>
     <fieldset class="twoColumn">
            <legend><%= Resources.Strings.S55 %></legend>
            <ol>
             <li>
                    <%= Html.LabeledTextBox("Street", Resources.Strings.S56)%>
                </li>
                <li>
                    <%= Html.LabeledTextBox("PostCode", Resources.Strings.S59)%>
                </li>
                 <li>
                    <%= Html.LabeledTextBox("Building", Resources.Strings.S57)%>
                </li>
                <li>
                    <%= Html.LabeledTextBox("City", Resources.Strings.S60)%>
                </li>
               
               
                <li>
                    <%= Html.LabeledTextBox("Flat", Resources.Strings.S58)%>
                </li>
                <li>
                    <%= Html.LabeledTextBox("Post", Resources.Strings.S61)%>
                </li>
               
            </ol>
        </fieldset>
        <fieldset><legend><%= Resources.Strings.S62 %></legend>
        <ol>
                <li>
                    <%= Html.LabeledTextArea("Notes", Resources.Strings.S63, 2, 35, null)%>
                </li>
                </ol></fieldset>
    
    <fieldset class="submit">
       <input type="submit" id="submit1" name="Submit" value='<%= Resources.Strings.S64 %>' />
            <input type="submit" id="submit2" name="Submit" value='<%= Resources.Strings.S65 %>'  />
    </fieldset>
    <%} %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsContent" runat="server">
</asp:Content>
