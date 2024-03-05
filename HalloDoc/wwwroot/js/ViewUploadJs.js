
//View Upload Js
$("#selectAll").click(function () {
    $(".selectFile").each(function () {
        if ($("#selectAll").prop('checked')) {
            $(this).prop('checked', true);
        } else {
            $(this).prop('checked', false);
        }
    })
})

const actualBtn = document.getElementById('actualFileBtn');

        const fileChosen = document.getElementById('fileChoosen');

        actualBtn.addEventListener('change', function () {
            fileChosen.textContent = this.files[0].name
        })

        $(".DeleteFile").click(function () {
            var ReqClientId, FileName ;
            if ($(this).closest('tr').length > 0) {
                ReqClientId = $(this).closest("tr").find(".ReqClientId").val();
                FileName = $(this).closest("tr").find(".FileName").html();
            } else {
                ReqClientId = $(this).siblings(".ReqClientId").val();
                FileName = $(this).closest("div").find(".FileName").text().trim();
            }

            $.ajax({
                url: '/AdminDashboard/DeleteFile',
                type: 'POST',
                data: { ReqClientId: ReqClientId, FileName: FileName },
                success: function (ReqClientId) {
                    location.reload();
                }
            })
        })

$("#DeleteAll").click(function(){ 
    var reqClientId, FileName;
    $(".selectFile").each(function(){
        if($(this).prop("checked")){
            if($(this).closest('tr').length > 0){
                reqClientId = $(this).closest("tr").find(".ReqClientId").val();
                        FileName = $(this).closest("tr").find(".FileName").html();
            }else{
                reqClientId = $(this).siblings(".ReqClientId").val();
                        FileName = $(this).closest("div").find(".FileName").text().trim();
            }

            $.ajax({
                url: '/AdminDashboard/DeleteFile',
                type: 'POST',
                data: {
                    reqClientId: reqClientId, FileName: FileName
                }
            })
        }
    });
    location.reload();
});