"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chat/signalr").build();
$("#send").disabled = true;

connection.on("ReceiveMessage", function (group, user, message) {
    var msg = message.replace(/&/g, "&").replace(/</g, "<").replace(/>/g, ">");
    var li = $("<li></li>").html('From user: <b>' + user + "</b> to group/user: <b>" + group + "</b> the message: <b>" + msg + "</b>");
    li.addClass("list-group-item");
    $("#messagesList").append(li);
});

connection.start().then(function () {
    $("#send").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

$("#send").on("click", function (event) {
    var group = $("#group").val();
    var user = $("#user").val();
    var message = $("#message").val();
    connection.invoke("SendMessage", group, user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});