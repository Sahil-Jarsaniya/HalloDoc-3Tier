//Cancel Case
$("#ConfirmCancelBtn").click(function () {
    var addNote = $("#addNote").val();
    var casetag = $(".CaseTag").val();
    var reqClientId = $("#reqClientId").val();
    $.ajax({
        url: '/AdminDashboard/CancelCase',
        type: 'POST',
        data: { reqClientId: reqClientId, addNote: addNote, CaseTag: casetag },
        success: function (result) {
            $("#CloseModal").click();
            location.reload();
        },
        error: function (error) {
            console.log(error);
            alert('error fetching details')
        },
    })
});

//Block Case
$("#ConfirmBlockBtn").click(function () {
    var addNote = $("#addBlockNote").val();
    var reqClientId = $("#blockreqClientId").val();
    $.ajax({
        url: '/AdminDashboard/BlockCase',
        type: 'POST',
        data: { reqClientId: reqClientId, addNote: addNote },
        success: function (result) {
            $("#CloseModal").click();
            location.reload();
        },
        error: function (error) {
            console.log(error);
            alert('error fetching details')
        },
    })
})

//Region and Physicians Filter
$(".RegionSelect").change(function () {
    var selectedRegion = $(this).val();
    console.log("assign")
    $.ajax({
        url: '/AdminDashboard/FilterPhysician',
        method: 'POST',
        data: {
            region: selectedRegion
        },
        success: function (response) {
            $("#PhysicianSelect").empty();
            $.each(response, function (index, doctor) {
                $("#PhysicianSelect").append($('<option>').text(doctor.physicians).val(doctor.physicianId));
            })
        }
    })
})

//Assign Case 
$("#ConfirmAssignBtn").click(function () {
    var addNote = $("#addAssignNote").val();
    var reqClientId = $("#AssignreqClientId").val();
    var PhysicianSelect = $(".PhysicianSelect").val();
    var RegionSelect = $(".RegionSelect").val();
    $.ajax({
        url: '/AdminDashboard/AssignCase',
        type: 'POST',
        data: { reqClientId: reqClientId, addNote: addNote, PhysicianSelect: PhysicianSelect, RegionSelect: RegionSelect },
        success: function (result) {
            $("#CloseModal").click();
            location.reload();
        },
        error: function (error) {
            console.log(error);
            alert('error fetching details')
        },
    })
})