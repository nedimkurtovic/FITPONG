
var connection = new signalR.HubConnectionBuilder().withUrl("/notifikacije").build();

document.addEventListener("DOMContentLoaded", function () {
    connection.start().catch(function (e) {
        return console.error(e.toString());
    });
});

connection.on("startaj", function (favoriti, tim1, tim2, id) {
    connection.invoke("PosaljiNotifikacije", favoriti, tim1, tim2, id).catch(function (err) {
        return console.error(err.toString());
    });
});

connection.on("PrimiNotifikacije", function (tim1, tim2, id) {
    var container = document.getElementById("notifikacije");

    var div = document.createElement("div");
    div.classList = "alert alert-success alert-dismissible";
    var close = '<a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>';
    var link = "https://localhost:44322/Takmicenje/Prikaz?id=" + id;
    var a = '<a href='+link+'>Evidentirana je utakmica izmedu '+tim1+' i '+tim2+'</a>';
    div.innerHTML += a;
    div.innerHTML += close;

    container.appendChild(div);
});




