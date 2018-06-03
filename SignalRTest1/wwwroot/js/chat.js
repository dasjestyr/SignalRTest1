const connection = new signalR.HubConnectionBuilder()
    .withUrl("/messaging")
    .build();

// When a message called ReceiveMessage is received, add it to the UI as a list item
connection.on("ReceiveMessage",
    (user, message) => {
        const msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
        const encodedMsg = user + " says " + msg;
        const li = document.createElement("li");
        li.textContent = encodedMsg;
        document.getElementById("messagesList").appendChild(li);
    });

connection.on("ReceiveHeartbeat",
    (message) => {
        console.log(message);
        const li = document.createElement("li");
        li.textContent = message;
        document.getElementById("messagesList").appendChild(li);
    });

connection.on("PushUpdate",
    (message) => {
        console.log(message);
        const li = document.createElement("li");
        li.textContent = message.message;
        document.getElementById("messagesList").appendChild(li);
    });

connection.start().catch(err => console.error(err.toString()));

// when the button is clicked, prevent postback and call the SendMessage method on the server
document.getElementById("sendButton").addEventListener("click",
    event => {
        const user = document.getElementById("userInput").value;
        const message = document.getElementById("messageInput").value;
        connection.invoke("SendMessage", user, message).catch(err => console.error(err.toString()));
        event.preventDefault();
    });

document.getElementById("startButton").addEventListener("click", event => {
    connection.invoke("CauseUpdate").catch(err => console.error(err.toString()));
    event.preventDefault();
});