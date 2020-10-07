
$(document).ready(function () {
    //var TAG = ;
    $("#TAG").change(function (e) {
        $.post(baseUrl + "Home/CheckTAG", {
            BARCODE: $("#TAG").val()
        }).done(function (data) {
            var pr = $.parseJSON(data);
            if (data == "[]") {
                alert("ไม่พบข้อมูล");
                $("#TAG").val("").focus();
            }
            else {
                $("#Truck").focus();
            }
            // alert(pr[0]["Barcode"]);//
        });
    });
    $("#User").change(function (e) {
        $.post(baseUrl + "Home/CheckUser", {
            USER: $("#User").val()
        }).done(function (data) {
            var pr = $.parseJSON(data);
            if (data == "[]") {
                $("#User").val("").focus();
            }
            else {
                $("#User").val(pr[0]["Mem_Name"]);
            }
        });
    });
    $("#Save").click(function () {
        if ($("#TAG").val() != "" && $("#Truck").val() != "" && $("#User").val() != "") {
            $.post(baseUrl + "Home/SaveData", {
                BARCODE: $("#TAG").val(),
                TRUCK: $("#Truck").val(),
                USER: $("#User").val()
            }).done(function (data) {
                if (data == "S") {
                    alert("บันทึกสำเร็จ");
                }
                else {
                    alert("ข้อมูลไม่ถูกต้อง");
                }
            });
        }
        else {
            alert("กรุณากรอกข้อมูล");
        }
    });


});

