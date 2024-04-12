$(document).ready(function () {

    $('#checkEmail').on('focusout', function () {
        var email = $(this).val();
        $.ajax({
            url: '/Request/PatientCheckEmail',
            type: 'POST',
            data: { email: email },
            success: function (data) {
                if (!data.exists) {
                    $('#hiddendiv').show();
                    $("#hiddendiv input").attr("required", "true");
                }
                else {
                    $('#hiddendiv').hide();
                    $("#hiddendiv input").attr("required", "false");
                }
            }
        });
    });
});