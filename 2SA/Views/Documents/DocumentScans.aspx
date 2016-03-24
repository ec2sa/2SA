<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<eArchiver.Models.ViewModels.Documents.DocumentScansViewModel>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>2SA - Skany dokumentu</title>
    <style type="text/css">
        body
        {
            font-size: .70em;
            font-family: Arial, Helvetica, Sans-Serif;
            color: #444444;
        }
        fieldset
        {
            border: 1px solid #BFBAB0;
        }
        legend {
            color:#444444;
            font-size:1.2em;
            font-weight:bold;
            margin-left:2em;
            padding:0 0.5em;
        }
        ul.none
        {
            list-style-type:none;   
        }

        ul.none li
        {
            display:inline;
            padding:0.5em;
            font-size:1.1em;
        }
    </style>
</head>
<body>
    <div>
        <% Html.RenderPartial("DocumentScansUserControl", Model); %>
    </div>
</body>
</html>
