var i = 0;
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
                        '<a href="#"  class="C" id="' + pr[i]["PRO_Cus"] + '">' + pr[i]["PRO_Cus"] + '</a>'
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


    $("#SavePic").click(function () {

        $.post("../Home/SaveUpload", {
            WO: $("#WO").val(),
           

        }).done(function (data) {
            if (data == "S") {
                Upload();
                alert("บันทึกสำเร็จ");

                window.location = baseUrl + "Home/PicUpload";
            }
            else if (data == "N") { alert("กรุณากรอกข้อมูลให้ครบ", 'Error'); }

        });

    });

    function Upload() {

        var formData = new FormData($("#formUP")[0]);
        // var NAME = $('#Barcode').val();
        $.ajax({
            type: 'POST',
            url: "/Home/UploadFiles",
            data: formData,
            cache: false,
            contentType: false,
            processData: false
        }).done(function (data) {
        });

    }
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
});
