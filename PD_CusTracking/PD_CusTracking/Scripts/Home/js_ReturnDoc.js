var i = 0;
$(document).ready(function () {
    $("#WO").focus();
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
            $.post(baseUrl + "Home/SaveDOC", {
                WO: $("#WO").val(),
                CUS: $("#Cus").val()
            }).done(function (data) {
                if (data == "S") {
                    Upload();
                    alert("บันทึกสำเร็จ");
                    window.location = baseUrl + "Home/ReturnDoc";
                }

                else { 
                }
            }); 
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
    function Upload() {
        var formData = new FormData($("#formUP")[0]);
        $.ajax({
            type: 'POST',
            url: "../Home/UploadFiles",
            data: formData,
            cache: false,
            contentType: false,
            processData: false
        }).done(function (data) {
        });
    }
});


