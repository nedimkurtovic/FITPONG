function GetIme() {
    $.get("/Account/GetPrikaznoIme", function (rezultz) {
        $("#PrikaznoIme").html(rezultz);
    })
}
GetIme();