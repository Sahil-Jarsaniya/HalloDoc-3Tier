/*var dashboardStatus = $("#DashboardStatus").val();*/
var status = localStorage.getItem("status");
$(".status-tab").click(function () {
    $(this).addClass("active");
    $(this).children("svg").removeClass("d-none");

    $('.status-tab').not(this).children("svg").addClass("d-none");
    $('.status-tab').not(this).removeClass("active");

    var id = $(this).attr('id');
    document.getElementById("DashboradForm").reset();

    if (id == 'status-new-tab') {
      /*  dashboardStatus = 1;*/
        localStorage.setItem("status", 1);
        //$("#DashboardStatus").val(1);
        $('#status-text').text('(New)');
    }
    else if (id == 'status-pending-tab') {
        //dashboardStatus = 2;
        localStorage.setItem("status", 2);
        //$("#DashboardStatus").val(2);
        $('#status-text').text('(Pending)');
    }
    else if (id == 'status-active-tab') {
        //dashboardStatus = 8;
        localStorage.setItem("status", 8);
        //$("#DashboardStatus").val(8);
        $('#status-text').text('(Active)');
    }
    else if (id == 'status-conclude-tab') {
        //dashboardStatus = 4;
        localStorage.setItem("status", 4);
        //$("#DashboardStatus").val(4);
        $('#status-text').text('(Conclude)');
    }
    else if (id == 'status-to-close-tab') {
        //dashboardStatus = 5;
        localStorage.setItem("status", 5);
        //$("#DashboardStatus").val(5);
        $('#status-text').text('(To Close)');
    }
    else if (id == 'status-unpaid-tab') {
        //dashboardStatus = 13;
        localStorage.setItem("status", 13);
        //$("#DashboardStatus").val(13);
        $('#status-text').text('(Unpaid)');
    }

    var status = localStorage.getItem("status");
    $.ajax({
        url: "/AdminDashboard/PartialTable",
        type: 'POST',
        data: { status: status },
        success: function (result) {
            console.log(result);
            $('#PartialTable').html(result);
        },
        error: function (error) {
            console.log(error);
            alert('error fetching details')
        },
    });

});
if (status == 8) {
    $("#status-active-tab").click();
}
else if (status == 2) {
    $("#status-pending-tab").click();
}
else if (status == 4) {
    $("#status-conclude-tab").click();
}
else if (status == 5) {
    $("#status-to-close-tab").click();
}
else if (status == 13) {
    $("#status-unpaid-tab").click();
}
else {
    $("#status-new-tab").click();
}


//region filter
$("#RegionFilter").change(function () {
    var RegionId = $(this).val();
    var Name = $("#search").val();
    var reqType = $("#filterReqType").val();
    var status = localStorage.getItem("status");
    var page = $("#pagenumber").val();
    $.ajax({
        url: '/AdminDashboard/PartialTable',
        type: 'POST',
        data: { RegionId: RegionId, Name: Name, status: status, reqType: reqType },
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
    var reqType = $("#filterReqType").val();
    var status = localStorage.getItem("status");
    var page = $("#pagenumber").val();
    $.ajax({

        url: '/AdminDashboard/PartialTable',
        type: 'POST',
        data: {
            status: status, Name: Name, ReginoId: ReginoId, reqType: reqType
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
    $("#filterReqType").val(reqType);
    var RegionId = $("#RegionFilter").val();
    var Name = $("#search").val();
    var status = localStorage.getItem("status");
    var page = $("#pagenumber").val();
    $.ajax({

        url: '/AdminDashboard/PartialTable',
        type: 'POST',
        data: {
            status: status, Name: Name, RegionId: RegionId, reqType: reqType
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



//Export
$("#ExportBtn").click(function () {
    var cloneaTable = $("#PartialTable").clone();
    cloneaTable.find(" td:nth-last-child(1), td:nth-child(2)").remove();
    cloneaTable.find(".mobileView").remove();
    cloneaTable.find(".pageMenu").remove();
    $("input[name='GridHtml']").val(cloneaTable.html());
});

//Export all
$("#ExportBtnAll").click(function () {
    var status = localStorage.getItem("status");
    $("#exportStatus").val(status);
    $("#exportAllForm").submit();
})


