<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage"%>
<%@ Import Namespace="eArchiver.Helpers" %>
<asp:Content ID="indexTitle" ContentPlaceHolderID="TitleContent" runat="server">
    2SA - Strona główna 
</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
   <h2><%--<%= Html.Encode(ViewData["Message"]) %>--%></h2>
    
       
       <table class="center" cellpadding="5px" cellspacing="5px">
        <tr>
            <td align="center"><h3>Sprawne archiwum, sprawne działanie</h3></td>
            <td align="center"><h3>Czym jest System Szybkiej Archiwizacji 2SA?</h3></td>
            <td align="center"><h3>Jakie korzyści zapewnia system 2SA?</h3></td>
        </tr>
        <tr>
            <td align="justify" style="padding:0 10px 0 10px;">  
                     Jeśli dotychczasowy sposób archiwizacji dokumentów w Twojej firmie nie gwarantuje szybkiego i łatwego dostępu do wszystkich informacji, zapewniając jednocześnie ich bezpieczeństwo oraz pełną nad nimi kontrolę System Szybkiej Archiwizacji jest produktem przeznaczonym dla Ciebie.
            </td>
            <td align="justify" style="padding:0 10px 0 10px;">
            To system służący do elektronicznej archiwizacji wszystkich rodzajów dokumentów, zarówno tradycyjnych, jak i elektronicznych. Elektroniczna centralizacja archiwalnych dokumentów za pośrednictwem 2SA     zapewnia szybki, łatwy oraz zdalny dostęp do pełnej dokumentacji prowadzonej w Twojej firmie, znacząco wpływając na szybkość przepływu informacji, a dzięki temu na wydajność i sprawność funkcjonowania firmy.
            </td>
            <td align="justify" style="padding:0 10px 0 10px;">
            Oprócz znacznego obniżenia kosztów utrzymywania archiwum, system 2SA niesie za sobą szereg korzyści, wynikających z uzyskiwanej za jego pośrednictwem szybkości i prostoty, łatwości dostępu i przejrzystości oraz bezpieczeństwa i kontroli.
            </td>
        </tr>
        </table>
        <table class="center" style="text-align:center;">
        <tr>
             <td style="padding:0em 1em;"><h3>Przed</h3></td>
             <td style="padding:0em 1em;" align="center"><h3>Po</h3></td></tr>
        <tr>
            <td style="padding:0em 1em;"><img alt="before 2SA" src='<%=Url.Content("~/Content/images/before.jpg") %>' /></td>
            <td style="padding:0em 1em;"><img alt="after 2SA" src='<%=Url.Content("~/Content/images/after.jpg") %>' /></td>
        </tr>
        <tr>
        <td colspan="2">
        <a class="howtoLink" onclick="window.open(this);return false;"  href='<%=Url.Content("~/Content/howto/[2SA]Tworzenie_dokumentu.pdf") %>'>Tworzenie dokumentów</a>
        <a class="howtoLink" onclick="window.open(this);return false;"  href='<%=Url.Content("~/Content/howto/[2SA]Edycja_dokumentu.pdf") %>'>Edycja dokumentów</a>
        <a class="howtoLink" onclick="window.open(this);return false;" href='<%=Url.Content("~/Content/howto/[2SA]Wyszukiwanie_i_przegladanie_dokumentow.pdf") %>'>Wyszukiwanie i przeglądanie dokumentów</a>
        </td>
        </tr>
        
       </table>
    
</asp:Content>


