
var konekcija = new signalR.HubConnectionBuilder().withUrl("/lampica").build();

konekcija.on("trenutnostanje", function (finalnisrc) {
    var lampa = document.getElementById("lampica");
    lampa.src = finalnisrc;
});

konekcija.on("PromjenaStatusa", function (imefajla) {
    var lampa = document.getElementById("lampica");
    lampa.src = imefajla;
});

konekcija.on("PromjenaPoruke", function (poruka) {
    var lampicaPoruka = document.getElementById("lampicaPoruka");
    lampicaPoruka.innerHTML = poruka;
});

document.addEventListener("DOMContentLoaded", function () {
    konekcija.start().then(() => {
        konekcija.invoke("VratiTrenutno").catch(function (err) {
            return console.error(err.toString());
        });
    });
});

document.getElementById("lampica").addEventListener("click", function (event) {
    var trenutnaSlika = document.getElementById("lampica").getAttribute("src");
    $.ajax({
        type: "GET",
        url: "/Account/GetPrikaznoIme",
        success: function (rezultz) {
          
            konekcija.invoke("PromijeniStanje", trenutnaSlika,rezultz).catch(function (err) {
                return console.error(err.toString());
            });
            event.preventDefault();
        }
    })
    
});

