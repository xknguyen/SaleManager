// Display Ajax Loading
$('html, body').ajaxStart(function () {
    $('#ajax_loading, #opacity_full_page').fadeIn(100);
});
$('html, body').ajaxStop(function () {
    $('#ajax_loading, #opacity_full_page').fadeOut(100);
});

$('.category h2').click(function () {
    $('.category ul').toggleClass('hide show fadeInUp');
});


// Sub

$('a.main-cate').on('mouseover',function (e) {
    e.preventDefault();
    $(this).next('.sub').toggleClass('hide show fadeInUp');
})

/* ===== Navbar Search ===== */

$('#navbar-search > a').on('click', function() {
  $('#navbar-search > a > i').toggleClass('fa-search fa-times');
  $("#navbar-search-box").toggleClass('show hidden animated fadeInUp');
  return false;
});

/* ===== Navbar Submenu ===== */

/**
 * Dropdown submenu positioning (left or right)
 */

$('ul.dropdown-menu a[data-toggle=dropdown]').hover(function() {
  var menu = $(this).parent().find("ul");
  var menupos = menu.offset();
  if ((menupos.left + menu.width()) + 30 > $(window).width()) {
    $(this).parent().addClass('pull-left');   
  }
  return false;
});

/* ===== Thumbs rating ===== */

$('.rating .voteup').on('click', function () {
  var up = $(this).closest('div').find('.up');
  up.text(parseInt(up.text(),10) + 1);
  return false;
});
$('.rating .votedown').on('click', function () {
  var down = $(this).closest('div').find('.down');
  down.text(parseInt(down.text(),10) + 1);
  return false;
});

/* ===== Responsive Showcase ===== */

$('.responsive-showcase ul > li > i').on('click', function() {
  var device = $(this).data('device');
  $('.responsive-showcase ul > li > i').addClass("inactive");
  $(this).removeClass("inactive");
  $('.responsive-showcase img').removeClass("show");
  $('.responsive-showcase img').addClass("hidden");
  $('.responsive-showcase img' + device).toggleClass("hidden show");
  $('.responsive-showcase img' + device).addClass("animated fadeIn");
  return false;
});

/* ===== Services ===== */

$('.service-item').hover (function() {
  $(this).children("i").toggleClass("fa-rotate-90");
  return false;
});

/*
*  Chọn số lượng sản phẩm
*  Author: Nguyễn Văn Thương
*
*/
function onlyNumbers(evt) {
    // SOME OPTIONS LIKE ENTER, BACKSPACE, HOME, END, ARROWS, ETC.
    var arrayExceptions = [8, 13, 16, 17, 18, 20, 27, 35, 36, 37,
        38, 39, 40, 45, 46, 144];
    if ((evt.keyCode < 48 || evt.keyCode > 57) &&
            (evt.keyCode < 96 || evt.keyCode > 106) && // NUMPAD
            $.inArray(evt.keyCode, arrayExceptions) === -1) {
        return false;
    }
}

$('.price-block input[type="number"]').on('keydown', onlyNumbers);

/*
* Submmit nội dung bình luận
* Thương
*/
$('#comment-submit').click(function (e) {
    e.preventDefault();
    var subject = $('#comment-subject').val();
    if (subject.length == 0) {
        $('#comment-message').html("Tiêu đề không được bỏ trống");
        return;
    }
    var content = $('#comment-content').val();
    if (content.length == 0) {
        $('#comment-message').html("Nội dung không được bỏ trống");
        return;
    }
    var id = $('#hidden-comment').val();
    $.post('/SingleProduct/Comment', { "id": id, "subject": subject, "content": content }, function (data) {
        $('#comment-message').html(data);
        $('#comment-subject').val('');
        $('#comment-content').val('');
        window.setTimeout('location.reload()', 3000);
    });
});

/*
* Report comment xấu
*
*/
$('.report-comment').click(function () {
    var confirmResult = confirm("Bạn có chắc chắn muốn báo cáo nội dung bình luận này không?");
    if (confirmResult) {
        var id = $(this).attr('data-value');
        $.post('/SingleProduct/ReportComment', { "id": id }, function (data) {
            alert(data);
        });
    }
});

$('.user-profile').mouseover(function () {
    $("#profile-box").toggleClass('show hidden animated fadeInUp');
    return false;
});
//$('.user-profile').mouseout(function () {
//    $("#profile-box").toggleClass('show hidden animated fadeInUp');
//    return false;
//});

$('#sign-out').click(function () {
    $.post("/Account/LogOff",null,null,null);
});

$('.sumcart').click(function (e) {
    e.preventDefault();
    $(".cart-summary").toggleClass('show hidden animated fadeInUp');
    return false;
});


$(function () {
    // Document.ready -> link up remove event handler
    $(".removeItem").click(function () {
        // Get the id from the link
        var recordToDelete = $(this).attr("data-id");
        if (recordToDelete != '') {
            // Perform the ajax post
            $.post("/ShoppingCart/RemoveFromCart", { "id": recordToDelete },
                function (data) {
                    // Successful requests get here
                    // Update the page elements
                    if (data.ItemCount == 0) {
                        $('.row-' + data.DeleteId).fadeOut('slow');
                        $('.row-' + data.DeleteId).remove();
                    }
                    var $table = $('table>tbody').find('tr');
                    if ($table.length == 0) {
                        $('.checkout').remove();
                    }
                    $('.cart-count').text(data.CartCount);
                    $('.sum-total').text(data.CartTotal);
                    $('#update-message').text(data.Message);

                });
        }
    });
    $('#checkout').click(function () {
        var $table = $('table>tbody').find('tr');
        if ($table.length > 0) {

        }
    });
    $('.update-quantity').click(function () {
        var recordId = $(this).attr('data-id');
        var quantity = $('.qty-' + recordId).val();
        $.post("/ShoppingCart/UpdateQuantity",
        { "id": recordId, "quantity": quantity },
        function (data) {
            $('#update-message').text(data.Message);
            $('.sum-total').text(data.CartTotal);
            $('.cart-count').text(data.CartCount);
            $('.row-'+recordId).children('.qty').text('x '+quantity)
        });
    });
});