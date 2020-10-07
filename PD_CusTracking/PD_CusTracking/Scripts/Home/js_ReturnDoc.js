﻿var i = 0; 
$(document).ready(function () {
    $("#WO").focus();
    //var TAG = ;
    $("#WO").change(function (e) {
        document.getElementById('TAGCus').style.display = 'none';
        $('#data-table-basic').dataTable().fnClearTable();
        $.post(baseUrl + "Home/GETWODoc", {
            WO: $("#WO").val()
        }).done(function (data) {
            var pr = $.parseJSON(data);
            if (data != "[]") { 
                document.getElementById('TableDetai').style.display = '';
                $.each(JSON.parse(data), function (i, obj) {
                    $('#data-table-basic').dataTable().fnAddData([ 
                        pr[i]["Delivery_WO"], 
                        pr[i]["Delivery_Truck"],                        
                        '<a href="#"  class="C" id="' + pr[i]["PRO_Cus"] + '">' + pr[i]["PRO_Cus"]  + '</a>'
                    ]); 

                });
                Click()
               // $("#WO").val('').focus();
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
  
  
    $("#Save").click(function () {
        if ($("#WO").val() != "" && $("#User").val() != "" && i > 0) {
            for (var a = 0; a < i; a++) {
                $.post(baseUrl + "Home/SaveReturn", {
                    BARCODE: $("#" + a).html(),
                    WO: $("#WO").val(),
                    // TRUCK: $("#Truck").val(),
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
        $("#User").focus();
    });
}
function ClickDelete() {
    $(".Delete").click(function () {
        document.getElementById(this.id).remove();
        i -= 1;
        //  alert(i);
    });
}
