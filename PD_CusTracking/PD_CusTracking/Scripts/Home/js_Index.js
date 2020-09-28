$(document).ready(function () {
    $("#Receive").click(function () { //ทำงานหลังจากกดปุ่ม
       // alert($("#Receive").html());
        window.location = baseUrl + "Home/Receive";  //เปิดหน้าที่ต้องการ
       // $("form").submit(function () { return false; });//ไม่ให้FromLoadหลังกด Enter Input
    });
    $("#Delivery").click(function () { //ทำงานหลังจากกดปุ่ม
        alert("ยังไม่มีหน้า");
        // window.location = baseUrl + "PDReturn/PDReturn";  //เปิดหน้าที่ต้องการ
        // $("form").submit(function () { return false; });//ไม่ให้FromLoadหลังกด Enter Input
    });
});

/*
     $.post(baseUrl + "Request/SaveRQ", {
                    MACHIN: $("#Machin").val(),
                    BAR_KAN: selectobject.options[i].value,
                    QTY: $("#Qty").val(),
                    USER: $("#User").val(),
                    TIME_ACTUAL: $("#Time_Actual").val(),
                    PROCESS_SPEED: $("#Process_Speed").val()
                }).done(function (data) {

                });
 */