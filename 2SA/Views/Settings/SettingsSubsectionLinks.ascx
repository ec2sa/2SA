<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="eArchiver.Helpers"%>
<ul class="subsections">
<%if (eArchiver.Controllers.AppContext.IsAdmin())
  { %>
    <%= Html.SubsectionLink("Nag��wki", "Headers", "Settings", null, null, "currentSubsection", false)%>
    <%= Html.SubsectionLink("S�ownik " + Html.Headers().Rodzajow, "Types", "Settings", null, null, "currentSubsection", false)%>
    <%= Html.SubsectionLink("S�ownik " + Html.Headers().Kategorii, "Categories", "Settings", null, null, "currentSubsection", false)%>
    <%= Html.SubsectionLink("S�ownik " + Html.Headers().Typow, "Types2", "Settings", null, null, "currentSubsection", false)%>
   <%= Html.SubsectionLink("OCR", "OCRService", "Settings", null, null, "currentSubsection", false)%>
<%} %>
<%if (eArchiver.Controllers.AppContext.AllowInfoTypeTwoRead())
  { %>
    <%= Html.SubsectionLink(Resources.Strings.S37, "Senders", "Settings", null, null, "currentSubsection", false)%>
<%} %>
</ul>
