function isDateValid(el) {
    if (!el)
        return;
    var dateToCheck = el.value;
    if (!dateToCheck || dateToCheck == '')
        return;
    if (!isDate(dateToCheck, 'yyyy-MM-dd')) {
        alert(dateToCheck + ' nie jest poprawną datą!');
        $(el).val('');
        $(el).focus();
    }
}

jQuery.extend(
        jQuery.expr[':'].Contains = function(a, i, m) {
        return (a.textContent || a.innerText ||jQuery(a).text()||'').toLowerCase().indexOf(m[3].toLowerCase())>=0;
        });

$.fn.fillSelect = function(data, optionLabel) {
    var options = "<option value=''>" + optionLabel + "</option>";
    if (data.length > 0) {
        for (p in data) {
            var item = data[p];
            
            if (item.Selected == true) {
                options += "<option selected='selected' value='" + item.Value + "'>" + item.Text + "</option>";
            }
            else {
                options += "<option value='" + item.Value + "'>" + item.Text + "</option>";
            }
        }
    }
    this.html(options);
}

$.fn.fillTypeTable = function(data) {
    $(this).removeItems();
    var rows = "";
    if (data.length > 0) {
        for (p in data) {
            var item = data[p];
            rows += "<tr id='trTypeID_" + item.Value + "'><td>" + item.Text + "</td><td><a href='#' onclick='deleteType(" +item.Value+")'>x</a></td></tr>";
            //$("#trTypeID_" + item.Value + " a", this).bind("onclick", deleteType(item.Value));
        }
        rows += $("tr:last", this).html();
        $("tr:last", this).replaceWith(rows);

        

    }

}

$.fn.removeItems = function() {
    var rows = "<tr>" + $("tr:first", this).html() + "</tr>";
    rows += "<tr>" + $("tr:last", this).html() + "</tr>";
    $(this).html(rows);
}

//jquery debug ;)
function notify(info) {
    alert(info);
};

$(function() {

    $("#CategoryID").change(function() {
        var category = $("#CategoryID option:selected").attr("value");
        if (category.length > 0) {

            $.getJSON(rootPath + "/Custom/GetTypeByCategoryID?categoryID=" + category,

                    function(data) {
                        $("#TypeID").fillSelect(data, '---');
                    });
        }
        else {
            var option = "<option value=''>- ? -</option>";
            $("#TypeID").html(option);
        }
    });

    $("#SaveSender").livequery("click", function(e) {
        var firstName = $("#FirstName").val();
        var lastName = $("#LastName").val();
        var company = $("#Company").val();
        var position = $("#Position").val();
        var email = $("#Email").val();
        var webpage = $("#Webpage").val();

        var phonehome = $("#PhoneHome").val();
        var phonemobile = $("#PhoneMobile").val();
        var phonework = $("#PhoneWork").val();
        var faxwork = $("#FaxWork").val();

        var postCode = $("#PostCode").val();
        var post = $("#Post").val();
        var city = $("#City").val();
        var street = $("#Street").val();
        var building = $("#Building").val();
        var flat = $("#Flat").val();
        var notes = $("#Notes").val();

        var errormsg = "<ul>";
        var valid = true;

        var isFirstName = $("#FirstName").val().length > 0;
        var isLastName = $("#LastName").val().length > 0;
        var isCompany = $("#Company").val().length > 0;

        if (isCompany == false) {
            if (isFirstName == false || isLastName == false) {
                valid = false;
                errormsg += "<li class='error' >Podaj imię i nazwisko lub nazwę firmy.</li>";
            }
        }
        errormsg += "</ul>";
        if (valid) {
            $.getJSON(rootPath + "/Custom/CreateSender?firstName=" + firstName +
                "&lastName=" + lastName +
                "&company=" + company +
                "&position=" + position +
                "&email=" + email +
                "&webpage=" + webpage +
                "&phonehome=" + phonehome +
                "&phonemobile=" + phonemobile +
                "&phonework=" + phonework +
                "&faxwork=" + faxwork +
                "&postCode=" + postCode +
                "&post=" + post +
                "&city=" + city +
                "&street=" + street +
                "&building=" + building +
                "&flat=" + flat +
                "&notes=" + notes,
                function(data) {
                    $("#SenderID").fillSelect(data, "- wybierz nadawcę -");
                    $("#senderDialog").dialog('destroy');
                });
        }
        else {
            $("#validationSummary").attr("style", "display:block").addClass("errorMessage").html(errormsg);
        }
    });

    $("#CancelSender").click(function() {
        jQuery('#senderDialog').dialog('close');
        return false;
    });
});

function changeDictCategoryID() {
    var category = $("#DictCategoryID option:selected").attr("value");
    if (category.length > 0) {
        $.getJSON(rootPath+"/Custom/GetTypeByCategoryID?categoryID=" + category,
                    function(data) {
                        $("#TypesTable").fillTypeTable(data);
                    });
    }
    else {
        $("#TypesTable").removeItems();
    }
}

function addScanFromGrid(sender) {
    if (sender.html() == 'Dodaj') {
        var scanGuid = $('input[type="hidden"]:first', sender.parent()).val();

        var item = $("#tmpul > li").clone();
        item.toggleClass("imgPreview");
        $("div", item).toggleClass("editPreview");
        item.attr('id', 'li_' + scanGuid);

        $("div", item).append($('input[type="hidden"]:first', sender.parent()).clone());

        var link = $('a:first', sender.parent()).clone();
        var downloadLink = link.clone();
        downloadLink.html("Pobierz");
        link.attr("href", "#");
        //link.bind("onclick", "setPreview('" + scanGuid + "')");
        link.toggleClass("preview");
        
        var img = $('img:first', sender.parent()).clone();
        link.html(img);

        $("div", item).append(link);
        $("div", item).append("<a href='#' class='deleteLink' >x</a>");
        $("div", item).append("<br />");
        $("div", item).append(downloadLink);
        $("div > a.deleteLink", item).live("click", function() {
            var delBtn = $(this);
            var scanGuid = $('input[type="hidden"]:first', delBtn.parent()).val();
            $("#li_" + scanGuid).fadeOut(500, function() {
                $(this).remove();
                $(".scansGridItem > input[value=" + scanGuid + "]").parent().fadeIn("slow");
            });

            
        });
        $("div", item).append("<br /><br />");

//        var newVerList = $(document.createElement("a"));
//        newVerList.attr("href", "#");
//        newVerList.addClass("showDialog");
//        newVerList.bind("click", function() {
//            setVersionGuid(scanGuid);
//            jQuery('#scansDialog').dialog('open');
//            return false;
//        });
//        newVerList.html("Dodaj wersję z listy");
//        $("div", item).append(newVerList);
//        $("div", item).append("<br />");
//        
//        var newVer = $(document.createElement("a"));
//        newVer.attr("href", "#");
//        newVer.bind("click", function() {
//            setVersionGuid(scanGuid);
//            jQuery('#versionsDialog').dialog('open');
//            return false;
//        });
//        newVer.html("Dodaj wersję z dysku");
//        $("div", item).append(newVer);
        $("ul.imgPreview").append(item);
    }
    sender.parent().fadeOut("slow");
    
    return false;

};

function deleteType(id) {
    $.getJSON(rootPath+"/Custom/DeleteType?typeID=" + id,
        function(data) {
            if (data == true) {
                $("#trTypeID_" + id).fadeOut("slow");
            }
        });
    };

function ShowScan(guid) {
    
    $("#visibleScan").val(guid);
    if (guid == null)
        $("#visibleScan").val("");

    if (guid != null && guid.length > 0) {

        $.ajax({
            type: "POST",
            url: rootPath + "/Scans/RenderPreview",
            data: "scanGuid=" + guid,
            dataType: "html",
            success: function(result) {
                var domElement = $(result);
                $("#scanPreviewBox").html(domElement);
            }
        });
    }
    else {
        $("#scanPreviewBox").html("");
    }
}

function ShowScanRotated(guid, d) {

    $("#visibleScan").val(guid);
    if (guid == null)
        $("#visibleScan").val("");

    $.ajax({
        type: "POST",
        url: rootPath + "/Scans/RenderPreview",
        data: "scanGuid=" + guid + "&rotation=" + d,
        dataType: "html",
        success: function(result) {
            var domElement = $(result);
            $("#scanPreviewBox").html(domElement);
        }
    });
}

function ChangeClient() {
    var clientID = $("#ClientsDropDown").val();
    $.ajax({
        type: "POST",
        url: rootPath + "/Custom/ChangeClient",
        data: "clientID=" + clientID,
        dataType: "html",
        success: function() {
            top.location = rootPath;
        }

    });
    
}