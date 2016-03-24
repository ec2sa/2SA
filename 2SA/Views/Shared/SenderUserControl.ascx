<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="eArchiver.Helpers" %>
<script type="text/javascript">
    
    

</script>
    <div id="validationSummary" style="display:none">
    </div>
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
       <input type="button" id="SaveSender" value='<%= Resources.Strings.S64 %>' />
       <input type="button" id="CancelSender" value='<%= Resources.Strings.S65 %>' />
    </fieldset>
