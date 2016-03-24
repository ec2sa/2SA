$(".verWrapper").livequery("click", function() {
    var guid = $("#scanID", $(this).parent()).val();
    ShowScanVersion(guid);
});

function ShowScanVersion(guid) {
    $.ajax({
        type: "POST",
        url: rootPath + "/Documents/RenderVersionsBrowser",
        data: "scanGuid=" + guid,
        dataType: "html",
        success: function(result) {
            var domElement = $(result);
            $("#versionsBrowserBox").html(domElement);
        }
    });
}