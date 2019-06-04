//import { signalR } from '@aspnet/signalr';
//import { signalR,HubConnection } from '@aspnet/signalr-client'
import { HubConnectionBuilder } from "@aspnet/signalr";


class SignalRhub
{
    constructor(){
        //var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
        this.connection = new HubConnectionBuilder().withUrl("/chatHub").build();
    }

    Init = () =>
    {
        //Disable send button until connection is established
        document.getElementById("sendButton").style.disabled = true;

        this.connection.on("ReceiveMessage", function (user, message) {
            var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
            var encodedMsg = user + " says " + msg;
            var li = document.createElement("li");
            li.textContent = encodedMsg;
            document.getElementById("messagesList").appendChild(li);
        });

        this.connection.start().then(function () {
            document.getElementById("sendButton").disabled = false;
        }).catch(function (err) {
            return console.error(err.toString());
        });

        document.getElementById("sendButton").addEventListener("click", function (event) {
            var user = document.getElementById("userInput").value;
            var message = document.getElementById("messageInput").value;
            this.connection.invoke("SendMessage", user, message).catch(function (err) {
                return console.error(err.toString());
            });
            event.preventDefault();
        });
    }
}

export {SignalRhub}