<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<eArchiver.Models.ViewModels.Quota.QuotaViewModel>" %>

<h2>Wersja darmowa</h2>
    
    <div>
        Quota zota³a przekroczona
    </div>
    <div>
        <%= Html.Encode(Model.Message) %>
    </div>