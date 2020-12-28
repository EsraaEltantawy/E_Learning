
    $("body").on("click", "#btnPlay", function () {
        $("#btnPlay").hide();
                        var url = $("#txtUrl").val();
                        url = url.split('v=')[1];
                        $("#video").src = "//www.youtube.com/embed/" + url;
                        $("#video").show();
    });
