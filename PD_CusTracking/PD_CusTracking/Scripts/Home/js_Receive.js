$(function ()
    {
        $('#Save').click(function () { 
            var tag = $('#TAG').val();
            var truck = $('#Truck').val();
            var user = $('#User').val();
            if (tag != '' && truck != '' && user != '') {
                $.ajax

                    ({

                        type: 'POST',
                        url: 'Home/insertdata',
                        async: false,
                        data:''
                        { 'tag': '" + tag + "', 'truck': '" + truck + "', 'user': '" + user + "' } 
                        contentType: 'application/json; charset =utf-8',
                        success: function (data)
                        {
                            var obj = data.d;
                            if (obj == 'true') 
                            {
                                $('#TAG').val('');
                                $('#Truck').val('');
                                $('#User').val('');
                                alert("บันทึกข้อมูลเรียบร้อย");
                            }
                    },
                        error: function (result)
                    {
                            alert("บันทึกไม่สำเร็จ");
                    }
                 });
    } else 
    {
        alert("โปรดใส่ข้อมูลใหม่");
        return false;
    }  
}) 
    
            });  

