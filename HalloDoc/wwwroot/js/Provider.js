
//region filter
$("#RegionFilter2").change(function () {
    var RegionId = $(this).val();
    $.ajax({
        url: '/AdminDashboard/ProviderFilter',
        type: 'POST',
        data: { RegionId: RegionId },
        success: function (result) {
            $('#ProviderTable').html(result);
        },
        error: function (error) {
            console.log(error);
            alert('error fetching details')
        },
    })
});

function stopNoty(Physicianid) {
    $.ajax({
        url: '/AdminDashboard/StopNoty',
        type: 'POST',
        data: { Physicianid: Physicianid },
    })
}