$(document).ready(function () {


        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(showPosition);
        } 
    function showPosition(position) {
      
    var latitude = position.coords.latitude;
    var longitude = position.coords.longitude;
        console.log(latitude);
        console.log(longitude);
    $.ajax({
        url: '/PhysicianDashboard/PhysicianLocationUpdate',
        type: 'POST',
        data: {
            latitude: latitude, longitude: longitude
        }
    })
    }

})

var status = localStorage.getItem("status");
$(".status-tab").click(function () {
    $(this).addClass("active");
    $(this).children("svg").removeClass("d-none");

    $('.status-tab').not(this).children("svg").addClass("d-none");
    $('.status-tab').not(this).removeClass("active");

    var id = $(this).attr('id');
    document.getElementById("DashboradForm").reset();

    if (id == 'status-new-tab') {
        localStorage.setItem("status", 1);
        $('#stateName').text('(New)');
    }
    else if (id == 'status-pending-tab') {
        localStorage.setItem("status", 2);
        $('#stateName').text('(Pending)');
    }
    else if (id == 'status-active-tab') {
        //dashboardStatus = 8;
        localStorage.setItem("status", 8);
        //$("#DashboardStatus").val(8);
        $('#stateName').text('(Active)');
    }
    else if (id == 'status-conclude-tab') {
        //dashboardStatus = 4;
        localStorage.setItem("status", 4);
        //$("#DashboardStatus").val(4);
        $('#stateName').text('(Conclude)');
    }

    var status = localStorage.getItem("status");
    $.ajax({
        url: "/PhysicianDashboard/PartialTable",
        type: 'POST',
        data: { status: status },
        success: function (result) {
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
else {
    $("#status-new-tab").click();
}



//search box
$("#search").keyup(function (e) {
    var Name = $(this).val();
    var reqType = $("#filterReqType").val();
    var status = localStorage.getItem("status");
    var page = $("#pagenumber").val();
    $.ajax({

        url: '/PhysicianDashboard/PartialTable',
        type: 'POST',
        data: {
            status: status, Name: Name, reqType: reqType
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
    var Name = $("#search").val();
    var status = localStorage.getItem("status");
    var page = $("#pagenumber").val();
    $.ajax({

        url: '/PhysicianDashboard/PartialTable',
        type: 'POST',
        data: {
            status: status, Name: Name, reqType: reqType
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



$(document).ready(function () {
    $('#loader').hide();

    $(document).ajaxSend(function () {
        console.log("ajax start")
        $('#loader').show();
        $('#loader').fadeIn(240);
    });
    $(document).ajaxComplete(function () {
        console.log("ajax end")
        $('#loader').hide();
        $('#loader').addClass('d-none').fadeOut(230);
    });

})