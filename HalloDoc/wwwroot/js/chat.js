"use strict";
var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;
connection.on("ReceiveMessage", function (user, message, AccountTypeOfSender, AccountTypeOfReceiver, reqClientId) {
    var reqClientId1 = document.getElementById("reqClientId").value;
    var AccountTypeOfReceiver2 = document.getElementById("AccountTypeOfReceiver").value;
    var AccountType = document.getElementById("AccountTypeOfSender").value;
    if (reqClientId == reqClientId1 && ((parseInt(AccountType) + parseInt(AccountTypeOfReceiver2)) == (AccountTypeOfSender + AccountTypeOfReceiver))) {

        var li = document.createElement("li");
        document.getElementById("messagesList").appendChild(li);
        li.classList.add("p-1");
        li.classList.add("w-100");

        var time = new Date();
        var hour = time.getHours();
        var ampm = hour >= 12 ? 'PM' : 'AM';
        hour = hour % 12;
        hour = hour ? hour : 12;
        var minutes = time.getMinutes();
        var ActiveUser = document.getElementById("userInput").value;
        if (ActiveUser == user) {
            li.innerHTML = '<div class="d-flex">' +
                '<span class="border rounded m-1 p-1 w-auto msgSpan"><b>' + user + ':</b>' + message + '</span>' +
                '</div>' +
                '<span style="font-size: 10px;" class="timeSpan mx-1 d-flex">' + hour + ":" + minutes + " " + ampm + '</span>';

        } else {
            li.innerHTML = '<div class="d-flex justify-content-end">' +
                '<span class="border rounded m-1 p-1 w-auto msgSpan"><b>' + user + ':</b>' + message + '</span>' +
                '</div>' +
                '<span style="font-size: 10px;" class="timeSpan mx-1 d-flex justify-content-end">' + hour + ":" + minutes + " " + ampm + '</span>';
        }

        var b = document.getElementById("msgContainer");
        b.scrollTop = b.scrollHeight;
    }
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var reqClientId = document.getElementById("reqClientId").value;
    var senderId = document.getElementById("SenderId").value;
    var AccountType = document.getElementById("AccountTypeOfSender").value;
    var AccountTypeOfReceiver = document.getElementById("AccountTypeOfReceiver").value;
    var message = document.getElementById("messageInput").value;

    connection.invoke("SendMessage", user, message, parseInt(AccountType), parseInt(AccountTypeOfReceiver), parseInt(reqClientId)).catch(function (err) {
        return console.error(err.toString());
    });


    if (AccountType == 1) {

        $.ajax({
            url: '/AdminDashboard/StoreChat',
            type: 'POST',
            data: {
                reqClientId: reqClientId, senderId: senderId, message: message
            }
        })
    } else if (AccountType == 2) {
        $.ajax({
            url: '/PhysicianDashboard/StoreChat',
            type: 'POST',
            data: {
                reqClientId: reqClientId, senderId: senderId, message: message
            }
        })
    } else if (AccountType == 3) {
        $.ajax({
            url: '/Patient/StoreChat',
            type: 'POST',
            data: {
                reqClientId: reqClientId, senderId: senderId, message: message
            }
        })
    }
    document.getElementById("messageInput").value = '';
    event.preventDefault();

});

