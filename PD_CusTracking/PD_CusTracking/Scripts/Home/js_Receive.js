$(document).ready(function () {
    $("#TAG").change(function (e) { 
        $("#User").focus(); 
    }); 
    $("#User").change(function (e) {
     //   alert($("#User").val());
        $.post(baseUrl + "Home/CheckUser", {
            USER: $("#User").val()
        }).done(function (data) {
            var pr = $.parseJSON(data);
            if (data == "[]") {             
                $("#User").val("").focus();
            }
            else {
                $("#User").val(pr[0]["Mem_Name"]);
                $("#Truck").focus();
            }
        });
    }); 
});