﻿var i = 0;
$(document).ready(function () {
    $("#WO").focus();
    $("#WO").change(function (e) {
        document.getElementById('TableDetai').style.display = 'none';
        document.getElementById('TAGCus').style.display = '';

        $.post(baseUrl + "Home/GETWODoc", {
            WO: $("#WO").val()
        }).done(function (data) {
            var pr = $.parseJSON(data);
            if (data != "[]") {
                $("#Cus").val(pr[0]["PRO_Cus"]);
                $("#User").focus();
            }
            else {
                alert("ไม่พบข้อมูล WO นี้");
                $("#WO").val('').focus();
            }
        });
    });
    $("#User").change(function (e) {
        $("#TAG").focus();

    });
    $('#TAG').on('keydown', function (e) {
        if (e.key == 'Enter') {
            $.ajax({
                type: 'post',
                url: baseUrl + "Home/CheckBarPic",
                data: { BAR: $('#TAG').val() },
                success: function (data) {
                    if (data != "") {
                        $('#Barcodes').html(data);
                        $('#User').focus();
                    }
                    else {
                        alert("ไม่พบข้อมูลนี้", "Error");
                        $('#TAG').val("");
                    }
                }
            });
        }
    });
    $("#Save").click(function () { 
      //  if ($("#WO").val() != "" && $("#formUpLoadDoc").val() != "") {
            $.post(baseUrl + "Home/SaveDOC", {
                WO: $("#WO").val(),
                CUS: $("#Cus").val()
            }).done(function (data) {
                if (data == "S") {
                    UploadDoc();
                    UploadPic();
                    alert("บันทึกสำเร็จ");
                    window.location = baseUrl + "Home/ReturnDoc";
                }
                else {
                    alert("บันทึกไม่สำเร็จ");
                }
            });
       // }
    }); 

    function Click() {
        $(".C").click(function () {
            $.post(baseUrl + "Home/CheckTAG_ReturnDOC", {
                WO: $("#WO").val(),
                CUS: this.id
            });
            document.getElementById('TableDetai').style.display = 'none';
            document.getElementById('TAGCus').style.display = '';
            $("#Cus").val(this.id);
            $("#User").focus();

        });
    }
    function ClickDelete() {
        $(".Delete").click(function () {
            document.getElementById(this.id).remove();
            i -= 1;
        });
    }
    function UploadDoc() {
        var formData = new FormData($("#formUpLoadDoc")[0]);
        $.ajax({
            type: 'POST',
            url: "../Home/UploadFilesDoc",
            data: formData,
            cache: false,
            contentType: false,
            processData: false
        }).done(function (data) {
        });
    }
    function UploadPic() {
        var formData = new FormData($("#formUpLoadPic")[0]);
        $.ajax({
            type: 'POST',
            url: "../Home/UploadFilesPic",
            data: formData,
            cache: false,
            contentType: false,
            processData: false
        }).done(function (data) {
        });
    }
});


