<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<eArchiver.Models.ViewModels.Documents.DocumentSearchViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	2SA - Wyszukiwarka dokumentów
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2></h2>
    <% Html.RenderPartial("SearchControl", Model); %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsContent" runat="server">
<script type="text/javascript">
$(document).ready(function() {
        $("#senderSearch").keypress(function(e) {

                $('#SenderID option').each(function() {
                    $(this).attr("selected", "");
                });
                var text = $("#senderSearch").val();
                if (e.which == 32 || (65 <= e.which && e.which <= 65 + 25) || (97 <= e.which && e.which <= 97 + 25)) {
                    text = text + String.fromCharCode(e.which);
                }
                $('#SenderID option:Contains(' + text + '):first').attr("selected", "selected");
            });
        $("#Date").datepicker({dateFormat: 'yy-mm-dd'});
       
             $("#Date").blur(function(){
            isDateValid(this);
             });
        $("#CreateDate").datepicker({dateFormat: 'yy-mm-dd'});
     
             $("#CreateDate").blur(function(){
            isDateValid(this);
             });
         $("#DateTo").datepicker({dateFormat: 'yy-mm-dd'});
        
             $("#DateTo").blur(function(){
            isDateValid(this);
             });
        $("#CreateDateTo").datepicker({dateFormat: 'yy-mm-dd'});
        
             $("#CreateDateTo").blur(function(){
            isDateValid(this);
             });
        //setSortIcon();
        <%if (Model.IsSearchPerformed){ %>
        $("#accordion").accordion({ 
            collapsible: true,
            autoHeight: false, 
            active: false });
        <%}else{ %>
            $("#accordion").accordion({ collapsible: true });
        <%} %>
    });
</script>     
</asp:Content>
