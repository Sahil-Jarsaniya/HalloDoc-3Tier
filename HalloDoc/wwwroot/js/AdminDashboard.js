
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

//region filter
$("#RegionFilter").change(function () {
    var RegionId = $(this).val();
    var Name = $("#search").val();
    console.log(RegionId)
    $.ajax({
        url: '/AdminDashboard/PartialTable',
        type: 'POST',
        data: { RegionId: RegionId, Name: Name, status: dashboardStatus },
        success: function (result) {
            $('#PartialTable').html(result);
        },
        error: function (error) {
            console.log(error);
            alert('error fetching details')
        },
    })
});

//search box
$("#search").keyup(function (e) {
    /*  $('#DashboradForm').submit();*/
    var ReginoId = $("#RegionFilter").val();
    var Name = $(this).val();
    $.ajax({

        url: '/AdminDashboard/PartialTable',
        type: 'POST',
        data: {
            status: dashboardStatus, Name: Name, ReginoId: ReginoId
        },
        success: function (result) {
            $('#PartialTable').html(result);
        },
        error: function (error) {
            console.log(error);
            alert('error fetching details')
        },
    });
});


//filter by request Type
$(".filterReqByType").click(function () {
    var reqType = $(this).children().val()
    var RegionId = $("#RegionFilter").val();
    var Name = $("#search").val();
    console.log(reqType + " " + RegionId + " " + Name)
    $.ajax({

        url: '/AdminDashboard/PartialTable',
        type: 'POST',
        data: {
            status: dashboardStatus, Name: Name, RegionId: RegionId, reqType: reqType
        },
        success: function (result) {
            $('#PartialTable').html(result);
        },
        error: function (error) {
            console.log(error);
            alert('error fetching details')
        },
    });
})