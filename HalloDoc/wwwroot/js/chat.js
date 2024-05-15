"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `${user}: ${message}`;
    li.classList.add("m-1");
    li.classList.add("p-1");
    li.classList.add("border");
    li.classList.add("rounded");

    var b = document.getElementById("msgContainer");
    b.scrollTop = b.scrollHeight;

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
    var message = document.getElementById("messageInput").value;

    connection.invoke("SendMessage", user, message).catch(function (err) {
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
    }
    document.getElementById("messageInput").value = '';
    event.preventDefault();
});

