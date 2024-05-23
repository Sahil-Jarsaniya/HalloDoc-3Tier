"use strict";
var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable the send button until connection is established.
var sendButton = document.getElementById("sendButton");
if (sendButton) {
    sendButton.disabled = true;
}
connection.on("ReceiveMessage", function (user, message, AccountTypeOfSender, AccountTypeOfReceiver, reqClientId) {
    var reqClientId1 = document.getElementById("reqClientId").value;
    var AccountTypeOfReceiver2 = document.getElementById("AccountTypeOfReceiver").value;
    var AccountType = document.getElementById("AccountTypeOfSender").value;
    if (reqClientId == reqClientId1 && ((parseInt(AccountType) + parseInt(AccountTypeOfReceiver2)) == (AccountTypeOfSender + AccountTypeOfReceiver))) {
        var dateDivs = document.querySelectorAll('.dateDiv');
        var today = new Date();
        var dd = String(today.getDate())
        var mm = String(today.getMonth() + 1) //January is 0!
        var yyyy = today.getFullYear();
        today = dd + '/' + mm + '/' + yyyy;

        var date = '';
        if (dateDivs.length > 0) {
            var lastDiv = dateDivs[dateDivs.length - 1];
            date = lastDiv.querySelector('span').textContent;
            date = date.trim();
        }
        var divOfDate = '';
        if (date.toString() != today.toString()) {
            var divOfDate = '<div class="d-flex justify-content-center dateDiv">' + '<span>' + today + '</span>' + '</div>'
        }

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
            li.innerHTML = divOfDate +
                '<div class="d-flex rightMsg">' +
                '<span class="border  m-1 p-2 px-3 w-auto msgSpan">' + message + '</span>' +
                '</div>' +
                '<span style="font-size: 10px;" class="timeSpan mx-1 d-flex justify-content-end">' + hour + ":" + minutes + " " + ampm + '</span>';

        } else {
            li.innerHTML = divOfDate +
                '<div class="d-flex leftMsg">' +
                '<span class="border  m-1 p-2 px-3 w-auto msgSpan">' + message + '</span>' +
                '</div>' +
                '<span style="font-size: 10px;" class="timeSpan mx-1">' + hour + ":" + minutes + " " + ampm + '</span>';
        }
        var b = document.getElementById("msgContainer");
        b.scrollTop = b.scrollHeight;
    }

    if ((parseInt(AccountType) == AccountTypeOfReceiver) || (parseInt(AccountTypeOfReceiver2) == AccountTypeOfSender)) {
        Notification.requestPermission().then(perm => {
            if (perm === 'granted') {
                new Notification("HaloDoc", {
                    body: user + " says " + message,
                    image: "~/wwwroot/images/logo"
                })
            }
            else {
                console.log("aasa")
            }
        })
    }
});

connection.start().then(function () {
    if (sendButton) {
        sendButton.disabled = false;
    }
}).catch(function (err) {
    return console.error(err.toString());
});
if (sendButton) {

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
                    reqClientId: reqClientId, senderId: senderId, message: message, AccountTypeOfReceiver: parseInt(AccountTypeOfReceiver)
                }
            })
        } else if (AccountType == 2) {
            $.ajax({
                url: '/PhysicianDashboard/StoreChat',
                type: 'POST',
                data: {
                    reqClientId: reqClientId, senderId: senderId, message: message, AccountTypeOfReceiver: parseInt(AccountTypeOfReceiver)
                }
            })
        } else if (AccountType == 3) {
            $.ajax({
                url: '/Patient/StoreChat',
                type: 'POST',
                data: {
                    reqClientId: reqClientId, senderId: senderId, message: message, AccountTypeOfReceiver: parseInt(AccountTypeOfReceiver)
                }
            })
        }
        document.getElementById("messageInput").value = '';
        event.preventDefault();

    });

}
function closeChatBox() {
    document.getElementById("myChatBox").style.width = "0px";
    document.getElementById("myChatBox").innerHTML = "";
    connection.off("ReceiveMessage");
}

var input = document.getElementById("messageInput");

// Execute a function when the user presses a key on the keyboard
input.addEventListener("keypress", function (event) {
    // If the user presses the "Enter" key on the keyboard
    if (event.key === "Enter") {
        // Cancel the default action, if needed
        event.preventDefault();
        // Trigger the button element with a click
        document.getElementById("sendButton").click();
    }
}); 