var rotation = 0;

$(function() {
    $("#rotateLeft").livequery("click",function() {
        rotation = rotation + 3;
        var guid = $("#visibleScan").val();
        Rotate(guid, GetDirection());
    });

    $("#rotateRight").livequery("click",function() {
        rotation = rotation + 1;
        var guid = $("#visibleScan").val();
        Rotate(guid, GetDirection()); ;
    });

    $("#cancelRotation").livequery("click",function() {
        var guid = $("#visibleScan").val();
        Rotate(guid, 0);
    });

    $("#saveRotation").livequery("click",function() {
        var guid = $("#visibleScan").val();
        var d = GetDirection();
        $.get(rootPath + "/Scans/SaveRotated?scanGuid=" + guid + '&rotation=' + d,
            function() {
                location.reload();
            });
    });
});

function Rotate(guid, d) {
    ShowScanRotated(guid, d);
}

function ResetRotation() {
    rotation = 0;
}

function GetDirection() {
    var direction = rotation % 4;
    return direction;
}