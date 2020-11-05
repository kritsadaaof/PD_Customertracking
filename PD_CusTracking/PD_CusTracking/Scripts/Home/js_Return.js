var i = 0;
$(document).ready(function () {
    $("#WO").focus();
    $("#WO").change(function (e) {
        document.getElementById('TableDetai').style.display = 'none';
        document.getElementById('TAGCus').style.display = '';
        $.post(baseUrl + "Home/GETWO", {
            WO: $("#WO").val()
        }).done(function (data) {
            var pr = $.parseJSON(data);
            if (data != "[]") {
                $("#Cus").val(pr[0]["PRO_Cus"]);
                $("#TAG").focus();
            }
            else {
                alert("ไม่พบข้อมูล WO");
                $("#WO").val('').focus();
            } 
        });    
    });
    $("#TAG").change(function (e) {
        $.post(baseUrl + "Home/CheckTAG_Return", {
            WO: $("#WO").val(),
            CUS: $("#Cus").val(),
            BARCODE: $("#TAG").val()
        }).done(function (data) {
            var pr = $.parseJSON(data);
            if (data == "[]") {
                alert("ล้อนี้ไม่สามารถลงได้ เนื่องจากไม่ตรงกับลูกค้าที่เลือก");
                $("#TAG").val("").focus();
            }
            else {
                $('#TAGDetail').append('<a title="' + pr[0]["Delivery_TAG"] + '" class="Delete" id="' + i + '">' + pr[0]["Delivery_TAG"] + '</a>' + " ");
                i += 1;
                ClickDelete();
                $("#TAG").val('').focus();
            }
        });
    });

    $("#Save").click(function () {
        if ($("#WO").val() != "" && $("#User").val() != "" && i > 0) {
            for (var a = 0; a < i; a++) {
                $.post(baseUrl + "Home/SaveReturn", {
                    BARCODE: $("#" + a).html(),
                    WO: $("#WO").val(),
                    USER: $("#User").val()
                });

            }
            alert("บันทึกสำเร็จ");
            window.location = baseUrl + "Home/Return";
        }
        else {
            alert("กรุณากรอกข้อมูล");
        }
    });


});

function Click() {
    $(".C").click(function () {

        document.getElementById('TableDetai').style.display = 'none';
        document.getElementById('TAGCus').style.display = '';
        $("#Cus").val(this.id);
        $("#TAG").focus();
    });
}
function ClickDelete() {
    $(".Delete").click(function () {
        document.getElementById(this.id).remove();
        i -= 1;
    });
}