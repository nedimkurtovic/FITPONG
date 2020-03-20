var konekcija = new signalR.HubConnectionBuilder().withUrl("/chathub").build();

document.addEventListener("DOMContentLoaded", function () {
    konekcija.start();
});

konekcija.on("GetKonekcije", function (listakonekcija) {

    var listaUnliked = document.createElement("ul");
    for (var i = 0; i < listakonekcija.length; i++) {
        var dioliste = document.createElement("li");
        var linkic = document.createElement("a");
        linkic.innerText = listakonekcija[i].toString();
        linkic.href = "#";
        $(linkic).on("click", function (event) { KreirajAkoNema($(this)); });

        dioliste.appendChild(linkic);

        listaUnliked.appendChild(dioliste);
    }
    document.getElementById("ListaUsera").innerHTML = "Aktivni su <hr>";
    document.getElementById("ListaUsera").appendChild(listaUnliked);
});

$(document).on('keypress', function (e) {
    if (e.which == 13) {
        PosaljiPoruku(e);
        e.preventDefault();
    }
});
$("#batnposalji").on("click", function (event) {
    PosaljiPoruku(event);
})
function PosaljiPoruku(event) {
    //console.log($(".active"));
    //console.log($("textarea"));
    var Primatelj = $(".active")[0].innerText;
    var poruka = $("textarea")[0].value;
    $("textarea")[0].value = "";
    konekcija.invoke("PosaljiPoruku", poruka, Primatelj).catch(function (err) {
        console.error(err.toString());
    });
}

konekcija.on("PrimiPoruku", function (poruka, posiljatelj, drugiucesnik, vrijeme) {
    KreirajDiv(drugiucesnik);
    DodajTekst(drugiucesnik, poruka, posiljatelj, vrijeme);
})
//var sadrzajDivID = "sadrzaj" + usernameOciscen;
//var buttonId = "link" + usernameOciscen;
function DodajTekst(drugiucesnik, poruka, posiljatelj, vrijeme) {
    var usernameOciscen = drugiucesnik.replace(/[^a-zA-Z0-9+]+/gi, '');
    var id = "#sadrzaj" + drugiucesnik;
    //console.log($(id));
    $(id)[0].innerHTML += vrijeme + " | " + posiljatelj + " : " + poruka + "<br>";
    $(id)[0].scrollTop = $(id)[0].scrollHeight - $(id)[0].clientHeight;
}
function KreirajDiv(drugiucesnik) {
    if (drugiucesnik == "Main") {
        if (!document.getElementById("linkMain").className.includes("active"))
            document.getElementById("linkMain").className += " novaPoruka";
        return 0;
    }
    var usernameOciscen = drugiucesnik.replace(/[^a-zA-Z0-9+]+/gi, '');
    var sadrzajDivID = "sadrzaj" + usernameOciscen;
    var buttonId = "link" + usernameOciscen;

    var tempid = "#" + buttonId;
    var element = $(tempid);

    if (element.length == 0) {

        var batn = document.createElement("button");
        batn.className = "tablinks active novaPoruka";
        batn.setAttribute("onclick", "PrebaciTab(event, '" + sadrzajDivID + "','" + buttonId + "')");
        batn.innerText = drugiucesnik;
        batn.id = buttonId;

        //var BatnDiv = document.createElement("div");
        //BatnDiv.style.position = "relative";
        //var spanX = document.createElement("span");
        //spanX.innerText = "X";
        //spanX.className = "zatvori";

        //$(BatnDiv).append(spanX);
        //$(BatnDiv).append(batn);

        var prozorContent = document.createElement("div");
        prozorContent.className = "tabsadrzaj";
        prozorContent.id = sadrzajDivID;
        prozorContent.style.display = "none";

        $("#tabovi").append(batn);
        $("#sadrzajtabova").append(prozorContent);


        var ajdi = "#" + buttonId;
        $(ajdi).on("click", function (event) {
            PrebaciTab(event, sadrzajDivID, buttonId);
        });
        console.log("jel ovo ime");

        //$(ajdi).trigger("click");
    }
    else {
        if (!element.hasClass("active"))
            element[0].className += " novaPoruka";
    }
}