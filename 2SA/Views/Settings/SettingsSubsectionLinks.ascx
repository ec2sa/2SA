<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="eArchiver.Helpers"%>
<ul class="subsections">
<%if (eArchiver.Controllers.AppContext.IsAdmin())
  { %>
    <%= Html.SubsectionLink("Nag³ówki", "Headers", "Settings", null, null, "currentSubsection", false)%>
    <%= Html.SubsectionLink("S³ownik " + Html.Headers().Rodzajow, "Types", "Settings", null, null, "currentSubsection", false)%>
    <%= Html.SubsectionLink("S³ownik " + Html.Headers().Kategorii, "Categories", "Settings", null, null, "currentSubsection", false)%>
    <%= Html.SubsectionLink("S³ownik " + Html.Headers().Typow, "Types2", "Settings", null, null, "currentSubsection", false)%>
   <%= Html.SubsectionLink("OCR", "OCRService", "Settings", null, null, "currentSubsection", false)%>
<%} %>
<%if (eArchiver.Controllers.AppContext.AllowInfoTypeTwoRead())
  { %>
    <%= Html.SubsectionLink(Resources.Strings.S37, "Senders", "Settings", null, null, "currentSubsection", false)%>
<%} %>
</ul>
