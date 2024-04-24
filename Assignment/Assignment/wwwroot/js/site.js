// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


function EditTask(id) {
    $.ajax({
        url: '/Home/EditTask',
        type: 'GET',
        data: { id: id },
        success: function (res) {
            $("#ModalDiv").html(res);
            $('#EditTaskModal').modal('show'); 

        }
    })
}   

function AddTask() {
    $.ajax({
        url: '/Home/AddTask',
        type: 'GET',
        success: function (res) {
            $("#AddModalDiv").html(res);
            $("#AddTaskModal").modal('show');
        }
    })
}



