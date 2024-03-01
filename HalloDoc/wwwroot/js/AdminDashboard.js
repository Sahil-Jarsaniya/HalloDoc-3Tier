
//New Pending  ... css
var dashboardStatus = 0;
$(".status-tab").click(function () {
    $(this).addClass("active");
    $(this).children("svg").removeClass("d-none");

    $('.status-tab').not(this).children("svg").addClass("d-none");
    $('.status-tab').not(this).removeClass("active");

    var id = $(this).attr('id');
    

    if (id == 'status-new-tab') {
        dashboardStatus = 1;
        $('#status-text').text('(New)');
    }
    else if (id == 'status-pending-tab') {
        dashboardStatus = 2;
        $('#status-text').text('(Pending)');
    }
    else if (id == 'status-active-tab') {
        dashboardStatus = 8;
        $('#status-text').text('(Active)');
    }
    else if (id == 'status-conclude-tab') {
        dashboardStatus = 4;
        $('#status-text').text('(Conclude)');
    }
    else if (id == 'status-to-close-tab') {
        dashboardStatus = 5;
        $('#status-text').text('(To Close)');
    }
    else if (id == 'status-unpaid-tab') {
        dashboardStatus = 13;
        $('#status-text').text('(Unpaid)');
    }

    $.ajax({
        url: "/AdminDashboard/PartialTable",
        type: 'POST',
        data: { status: dashboardStatus },
        success: function (result) {
            $('#PartialTable').html(result);
        },
        error: function (error) {
            console.log(error);
            alert('error fetching details')
        },
    });

});

$("#status-new-tab").click();

//search box
$("#search").keyup(function (e) {
    /*  $('#DashboradForm').submit();*/
    var Name = $(this).val();
    $.ajax({

        url:'/AdminDashboard/PartialTable',
        type: 'POST',
        data: {
            status: dashboardStatus, Name: Name },
        success: function (result) {
            $('#PartialTable').html(result);
        },
        error: function (error) {
            console.log(error);
            alert('error fetching details')
        },
    })
})