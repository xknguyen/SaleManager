$(function () {
    $.fn.createTreeViewFolder = function (response, type) {
        $(this).html("");
        $(this).treeview({
            color: "#428bca",
            levels: 2,
            enableLinks: true,
            showBorder: false,
            data: response
        });

        $(this).on("click", "a", function (e) {
            e.preventDefault();
        });

        $(this).on("click", "li", function () {
            $("#fileContent").html("");
            var a = $(this).find("a").attr("href");
            var url = "/File/GetFile";
            $.ajax({
                type: "POST",
                url: url,
                data: {
                    path: a,
                    type: type
                }
            }).done(function (response) {
                var list = jQuery.parseJSON(response);
                for (var i = 0; i < list.length; i++) {
                    $("#fileContent").appendPicture(list[i]);
                }
            });
        });
    }
    $.fn.showPictureForm = function() {
        $(this).showMediaForm("picture");
    }

    $.fn.showVideoForm = function () {
        $(this).showMediaForm("video");
    }

    $.fn.showAllForm = function () {
        $(this).showMediaForm("all");
    }

    $.fn.showMediaForm = function(type) {
        $.ajax({
            type: "POST",
            url: "/File/GetDirectory"
        }).done(function(response) {
            $("#treeviewFolder").createTreeViewFolder(response, type);
            var li = $("#treeviewFolder").find("li").first();
            if (li != null)
                $(li).click();
            $("#showMediaForm").click();
        });
    }

    $.fn.appendPicture = function (data) {
        var datahtml = $("#fileBoxTemp").html();
        $(this).append(datahtml);
        var last = $(this).find(".file-box").last();
        var img = $(last).find("img").first();
        $(img).attr("alt", data.text);
        $(img).attr("title", data.text);
        $(img).attr("src", data.href);
    }

    $("#fileContent").on("click", "a.image-remove", function(e) {
        e.preventDefault();
        var img = $(this).closest("div.file").first().find("img").first();
       
        var src = $(img).attr("src");
        $.ajax({
            type: "POST",
            url: "/File/DeleteFile",
            data: {
                path: src
            }
        }).done(function(response) {
            if (response.Success) {
                var parent = $(img).closest("div.file-box").first();
                $(parent).remove();
            } else {
                alert(response.Message);
            }
        });
    });

    $("#fileContent").on("click", "img", function () {
        var thisClass = $(this).attr("class");
        if (thisClass.indexOf("selected") == -1) {
            $("#fileContent").find(".selected").each(function () {
                $(this).removeClass("selected");
            });
            $(this).addClass("selected");
        } else {
            $(this).removeClass("selected");
        }
    });

    $.fn.hidenBox = function(isHidden) {
        $(this).attr("class", "");
        $(this).addClass("row");
        $(this).addClass("animated");
        $(this).addClass(isHidden ? "fadeOutUp" : "fadeInUp");
        $("#newFolderName").val("");
        $('#frmCreatePic')[0].reset();
        if (isHidden) {
            $(this).addClass("hidden");
        }
    }

    // Tạo thư mục
    $("#createFolder").on("click", function(e) {
        e.preventDefault();
        $("#newFolderBox").hidenBox(false);
        if ($("#uploadBox").attr("class").indexOf("hidden") == -1) {
            $("#uploadBox").hidenBox(true);
        } else {
            var height = $("#newFolderBox").height();
            var panel = $("#fileContent").height();
            $("#fileContent").height(panel - height);
        }
        
    });

    $("#cancelNewFolder").on("click", function(e) {
        e.preventDefault();
        var height = $("#newFolderBox").height();
        var panel = $("#fileContent").height();
        $("#fileContent").height(panel + height);
        $("#newFolderBox").hidenBox(true);
    });

    $("#addNewFolder").on("click", function (e) {
        e.preventDefault();
        // Lấy tên thu muc mói
        var newName = $("#newFolderName").val();
        if (newName == null || newName == "") {
            $("#newFolderName").focus();
        } else {
            // lấy thư mục hiện tại
            var curLi = $("#treeviewFolder").find("li.node-selected").first();
            var path = $(curLi).find("a").first().attr("href");

            if (path != null) {
                path += "\\" + newName;
                $.ajax({
                    type: "POST",
                    url: "/File/CreateFolder",
                    data: {
                        path: path
                    }
                }).done(function(response) {
                    if (response.Success) {
                        $.ajax({
                            type: "POST",
                            url: "/File/GetDirectory"
                        }).done(function(response) {
                            $("#treeviewFolder").createTreeViewFolder(response, "picture");
                            var li = $("#treeviewFolder").find("li").first();
                            if (li != null)
                                $(li).click();
                            $("#newFolderBox").hidenBox(true);
                        });
                    } else {
                        alert(response.Message);
                    }
                });
            } else {
                alert("Bạn chưa chọn thư mục!");
            }
        }
    });

    $("#deleteFolder").on("click", function(e) {
        e.preventDefault();
        // lấy thư mục hiện tại
        var curLi = $("#treeviewFolder").find("li.node-selected").first();
        var path = $(curLi).find("a").first().attr("href");

        if (path != null) {
            $.ajax({
                type: "POST",
                url: "/File/DeleteFolder",
                data: {
                    path: path
                }
            }).done(function (response) {
                if (response.Success) {
                    $.ajax({
                        type: "POST",
                        url: "/File/GetDirectory"
                    }).done(function (response) {
                        $("#treeviewFolder").createTreeViewFolder(response, "picture");
                        var li = $("#treeviewFolder").find("li").first();
                        if (li != null)
                            $(li).click();
                    });
                } else {
                    alert(response.Message);
                }
            });
        } else {
            alert("Bạn chưa chọn thư mục!");
        }
    });


    // UploadFile
    $("#showUploadFile").on("click", function(e) {
        e.preventDefault();
        $("#uploadBox").hidenBox(false);
        var cla = $("#newFolderBox").attr("class");
        if (cla.indexOf("hidden") == -1) {
            $("#newFolderBox").hidenBox(true);
        } else {
            var height = $("#uploadBox").height();
            var panel = $("#fileContent").height();
            $("#fileContent").height(panel - height);
        }
    });
    $("#cancelUpload").on("click", function (e) {
        e.preventDefault();
        var height = $("#uploadBox").height();
        var panel = $("#fileContent").height();
        $("#fileContent").height(panel + height);
        $("#uploadBox").hidenBox(true);
    });

    $("#uploadButton").on("click", function (e) {
        e.preventDefault();
        if ($('#uploadFile').get(0).files.length === 0) {
            alert("Bạn chưa chọn file nào!");
        } else {
            var curLi = $("#treeviewFolder").find("li.node-selected").first();
            var path = $(curLi).find("a").first().attr("href");

            if (path != null) {
                $("#pictureFolder").attr("value",path);
                var form = new FormData($("#frmCreatePic")[0]);
                $.ajax({
                    url: "/File/Upload",
                    type: "POST",
                    data: form,
                    // this is the important stuff you need to override the usual post behavior
                    cache: false,
                    contentType: false,
                    processData: false,
                    success: function(data) {
                        if (data.Success) {
                            $("#fileContent").html("");
                            var url = "/File/GetFile";
                            $.ajax({
                                type: "POST",
                                url: url,
                                data: {
                                    path: path,
                                    type: "picture"
                                }
                            }).done(function (response) {
                                var list = jQuery.parseJSON(response);
                                for (var i = 0; i < list.length; i++) {
                                    $("#fileContent").appendPicture(list[i]);
                                }
                            });
                            $("#cancelUpload").click();
                        } else {
                            alert(data.Message);
                        }
                    }
                });
            }
        }
    });
});