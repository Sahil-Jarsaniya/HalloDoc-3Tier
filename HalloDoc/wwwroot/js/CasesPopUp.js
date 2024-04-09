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
            $(".CloseModal").click();
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
            $(".CloseModal").click();
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
    $.ajax({
        url: '/AdminDashboard/FilterPhysician',
        method: 'POST',
        data: {
            region: selectedRegion
        },
        success: function (response) {
            $(".PhysicianSelect").empty();
            $.each(response, function (index, doctor) {
                $(".PhysicianSelect").append($('<option>').text(doctor.physicians).val(doctor.physicianId));
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
            $(".CloseModal").click();
            location.reload();
        },
        error: function (error) {
            console.log(error);
            alert('error fetching details')
        },
    })
})

//Transfer Case Region filter
$(".TransferRegionSelect").change(function () {
    var selectedRegion = $(this).val();
    var selectedPhy = $("#PhyId").val();
    console.log(selectedPhy);
    $.ajax({
        url: '/AdminDashboard/FilterPhysician',
        method: 'POST',
        data: {
            region: selectedRegion, PhyId: selectedPhy
        },
        success: function (response) {
            $(".TransferPhysicianSelect").empty();
            $.each(response, function (index, doctor) {
                $(".TransferPhysicianSelect").append($('<option>').text(doctor.physicians).val(doctor.physicianId));
            })
        }
    })
})

//Transfer Case
$("#ConfirmTransferBtn").click(function () {
    var addNote = $("#addTransferNote").val();
    var reqClientId = $("#TransferClientId").val();
    var PhysicianSelect = $(".TransferPhysicianSelect").val();
    var RegionSelect = $(".TransferRegionSelect").val();
    $.ajax({
        url: '/AdminDashboard/TransferCase',
        type: 'POST',
        data: { reqClientId: reqClientId, addNote: addNote, PhysicianSelect: PhysicianSelect, RegionSelect: RegionSelect },
        success: function (result) {
            $(".CloseModal").click();
            location.reload();
        },
        error: function (error) {
            console.log(error);
            alert('error fetching details')
        },
    })
})

//Clear Case

$("#ConfirmClearBtn").click(function () {
    var reqClientId = $("#ClearClientId").val();
   
    $.ajax({
        url: '/AdminDashboard/ClearCase',
        type: 'POST',
        data: { reqClientId: reqClientId },
        success: function () {
            $(".CloseModal").click();
            location.reload();
        },
        error: function (error) {
            console.log(error);
            alert('error fetching details')
        },
    })
})

//Send Agreement

$("#AgreementConfirmBtn").click(function () {
    var id = $("#AgreementClientId").val();
    var phone = $("#AgreementPhone").val();
    var email = $("#AgreementEmail").val();

    $.ajax({
        url: '/AdminDashboard/SendAgreement',
        type: 'POST',
        data: {
            reqClientId: id, phone: phone, email:email 
        },
        success: function () {
            $(".CloseModal").click();
        },
        error: function (error) {
            console.log(error);
            alert('error fetching details')
        },
    })
});


$("#EncounerSavebtn").click(function () {
    var id = $("#EncounterId").val();
    var radioValue = $("input[name='options']:checked").val();
    console.log(radioValue);
    $.ajax({
        url: '/AdminDashboard/Encounter',
        type: 'POST',
        data: {
            reqClientId: id, option: radioValue
        },
        success: function () {
            $(".CloseModal").click();
            location.reload();
        },
        error: function (error) {
            console.log(error);
            alert('error fetching details')
        },
    })
});