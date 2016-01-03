$(document).ready(function () {
    var clickedFlag = false;
    $(".ratingStar").mouseover(function () {
        $(this).attr("src", "/Images/yellowstar.png");
        $(this).prevAll(".ratingStar").attr("src", "/Images/yellowstar.png");
    });
    $(".ratingStar, #radingDiv").mouseout(function () {
        $(this).attr("src", "/Images/silverstar.png");
    });
    $("#ratingDiv").mouseout(function () {
        if (!clickedFlag) {
            $(".ratingStar").attr("src", "/Images/silverstar.png");
        }
    });
    $(".ratingStar").click(function () {
        clickedFlag = true;
        $(".ratingStar").unbind("mouseout mouseover click").css("cursor", "default");

        var $rating = $(this).attr("data-value");
        var $id = $('#ratingDiv').attr('data-id');
        var $url = window.location.pathname;
        $.post('/SingleProduct/Voting', { "id": $id, "rating": $rating, "url" : $url }, function (data) {
            $("#lblResult").html(data);
        });

    });
    $("#lblResult").ajaxStart(function () {
        $("#lblResult").html("Đang xử lý ....");
    });
    $("#lblResult").ajaxError(function () {
        $("#lblResult").html("<br/>Lỗi");
    });
});