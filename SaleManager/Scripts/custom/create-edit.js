$(document).ready(function () {
    var currentEditor = null;
    var currentPicture = null;
        tinymce.init({
            selector: "textarea#Description",
            entity_encoding: "raw",
            theme: "modern",
            height: 150,
            menubar: false,
            plugins: [
                "link image lists charmap print preview hr anchor pagebreak spellchecker",
                "searchreplace wordcount visualblocks visualchars code fullscreen insertdatetime media nonbreaking",
                "save table contextmenu directionality emoticons template paste textcolor"
            ],
            toolbar: "insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist | link addPicture | forecolor backcolor",
            style_formats: [
                { title: "Bold text", inline: "b" },
                { title: "Red text", inline: "span", styles: { color: "#ff0000" } },
                { title: "Red header", block: "h1", styles: { color: "#ff0000" } },
                { title: "Example 1", inline: "span", classes: "example1" },
                { title: "Example 2", inline: "span", classes: "example2" },
                { title: "Table styles" },
                { title: "Table row 1", selector: "tr", classes: "tablerow1" }
            ],
            setup: function (editor) {
                editor.addButton('addPicture', {
                    icon: "image",
                    tooltip: "Thêm hình ảnh",
                    onclick: function () {
                        $(this).showPictureForm();
                        $("#selectMediaButton").attr("data-type", "tinymce");
                        currentEditor = editor;
                        currentPicture = null;
                    }
                });
            }
        });

    $("#selectMediaButton").on("click", function (e) {
        e.preventDefault();
        var img = $("#fileContent").find("img.selected").first();
        if (img.length != 0) {
            var type = $("#selectMediaButton").attr("data-type");
            switch (type) {
                case "tinymce":
                    if (currentEditor != null) {
                        // get img
                        var im = "<img src='" + $(img).attr("src") + "' data-mce-selected='1' height='300'>";
                        currentEditor.insertContent(im);
                    } else {
                        if (currentPicture != null) {
                            $("#choose-image").find("img").first().attr("src", $(img).attr("src"));
                            $("#ThumbImage").attr("value", $(img).attr("src"));
                        }
                    }
                    break;
            }
            $("#closeMediaButton").click();
        } else {
            alert("Bạn chưa chọn hình ảnh nào");
        }
    });

    $("#choose-image").on("click", function(e) {
        e.preventDefault();
        $(this).showPictureForm();
        $("#selectMediaButton").attr("data-type", "tinymce");
        currentEditor = null;
        currentPicture = this;
    });

    if ($(".i-checks")!=null && $(".i-checks").length !== 0) {
        $(".i-checks").iCheck({
            checkboxClass: "icheckbox_square-green"
        });
    }
});