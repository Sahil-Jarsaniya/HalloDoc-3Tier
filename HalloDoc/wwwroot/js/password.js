$(document).ready(function () {

    $('#checkEmail').on('focusout', function () {
        var email = $(this).val();

        var emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (emailRegex.test(email)) {

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
        } else {
            $('#hiddendiv').hide();
            $("#hiddendiv input").attr("required", "false");
        }
    });
});