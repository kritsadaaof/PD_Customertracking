var i = 0; 
$(document).ready(function () {
    $("#WO").focus();
    $("#WO").change(function (e) {
        $.post(baseUrl + "Home/CheckWO", {
            BARCODE: $("#WO").val()
        }).done(function (data) {
            var pr = $.parseJSON(data);
            if (data == "[]") {
                alert("ไม่พบข้อมูล");
                $("#WO").val("").focus();
            }
            else {
                $("#User").focus();
            }
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
                $("#TAG").focus();
            }
        }); 
    });
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
                $('#TAGDetail').append('<a title="' + pr[0]["Barcode"]+'" class="C" id="' + i + '">' + pr[0]["Barcode"] + '</a>'+" ");
                i += 1;
                Click();
               
                $("#TAG").val('').focus();
            } 
        });
    });

  

  
    $("#Save").click(function () {
       // var prWO;
        if ($("#WO").val() != "" && $("#User").val() != "" &&i>0) {            
                    for (var a = 0; a < i; a++) {
                        $.post(baseUrl + "Home/SaveData", {
                            BARCODE: $("#" + a).html(),
                            WO: $("#WO").val(),
                           // TRUCK: $("#Truck").val(),
                            USER: $("#User").val()
                        });

                    }
                    alert("บันทึกสำเร็จ");
                    location.reload(); 
        }
        else {
            alert("กรุณากรอกข้อมูล");
        }
    });


});

function Click() {   
    $(".C").click(function () {  
      document.getElementById(this.id).remove();
        i -= 1;
      //  alert(i);
    });
}

