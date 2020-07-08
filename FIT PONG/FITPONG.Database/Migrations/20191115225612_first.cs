using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FIT_PONG.Migrations
{
    public partial class first : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Brackets",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brackets", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Feeds",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(nullable: true),
                    DatumModifikacije = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feeds", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Gradovi",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gradovi", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Kategorije",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Opis = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kategorije", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Konverzacije",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DatumKreiranja = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Konverzacije", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Logins",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(nullable: false),
                    PasswordSalt = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logins", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Objave",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    DatumKreiranja = table.Column<DateTime>(nullable: false),
                    DatumIzmjene = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Objave", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Permisije",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permisije", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SistemiTakmicenja",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Opis = table.Column<string>(maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SistemiTakmicenja", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "StatusiTakmicenja",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Opis = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusiTakmicenja", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "StatusiUtakmice",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Opis = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusiUtakmice", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TipoviRezultata",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoviRezultata", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TipoviStatusaPoruke",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoviStatusaPoruke", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TipoviUtakmica",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoviUtakmica", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Uloge",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(maxLength: 20, nullable: false),
                    Opis = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uloge", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "VrsteTakmicenja",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VrsteTakmicenja", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Runde",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrojRunde = table.Column<int>(nullable: false),
                    DatumPocetka = table.Column<DateTime>(nullable: false),
                    BracketID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Runde", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Runde_Brackets_BracketID",
                        column: x => x.BracketID,
                        principalTable: "Brackets",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Useri",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(maxLength: 30, nullable: false),
                    Prezime = table.Column<string>(maxLength: 30, nullable: false),
                    DatumRodjenja = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(maxLength: 60, nullable: false),
                    GradID = table.Column<int>(nullable: false),
                    LoginID = table.Column<int>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    PrikaznoIme = table.Column<string>(maxLength: 50, nullable: true),
                    JacaRuka = table.Column<string>(maxLength: 8, nullable: true),
                    Visina = table.Column<double>(nullable: true),
                    BrojPosjetaNaProfil = table.Column<int>(nullable: true),
                    ProfileImagePath = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Useri", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Useri_Gradovi_GradID",
                        column: x => x.GradID,
                        principalTable: "Gradovi",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Useri_Logins_LoginID",
                        column: x => x.LoginID,
                        principalTable: "Logins",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "FeedsObjave",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FeedID = table.Column<int>(nullable: false),
                    ObjavaID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedsObjave", x => x.ID);
                    table.ForeignKey(
                        name: "FK_FeedsObjave_Feeds_FeedID",
                        column: x => x.FeedID,
                        principalTable: "Feeds",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_FeedsObjave_Objave_ObjavaID",
                        column: x => x.ObjavaID,
                        principalTable: "Objave",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Poruke",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DatumVrijeme = table.Column<DateTime>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    KonverzacijaID = table.Column<int>(nullable: false),
                    TipStatusaPorukeID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Poruke", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Poruke_Konverzacije_KonverzacijaID",
                        column: x => x.KonverzacijaID,
                        principalTable: "Konverzacije",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Poruke_TipoviStatusaPoruke_TipStatusaPorukeID",
                        column: x => x.TipStatusaPorukeID,
                        principalTable: "TipoviStatusaPoruke",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "UlogePermisije",
                columns: table => new
                {
                    UlogaID = table.Column<int>(nullable: false),
                    PermisijaID = table.Column<int>(nullable: false),
                    DatumPostavljanja = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UlogePermisije", x => new { x.UlogaID, x.PermisijaID });
                    table.ForeignKey(
                        name: "FK_UlogePermisije_Permisije_PermisijaID",
                        column: x => x.PermisijaID,
                        principalTable: "Permisije",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_UlogePermisije_Uloge_UlogaID",
                        column: x => x.UlogaID,
                        principalTable: "Uloge",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Takmicenja",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(maxLength: 50, nullable: false),
                    DatumPocetka = table.Column<DateTime>(nullable: false),
                    DatumZavrsetka = table.Column<DateTime>(nullable: false),
                    RokPocetkaPrijave = table.Column<DateTime>(nullable: false),
                    RokZavrsetkaPrijave = table.Column<DateTime>(nullable: false),
                    DatumKreiranja = table.Column<DateTime>(nullable: false),
                    BrojRundi = table.Column<int>(nullable: false),
                    MinimalniELO = table.Column<int>(nullable: false),
                    KategorijaID = table.Column<int>(nullable: false),
                    SistemID = table.Column<int>(nullable: false),
                    VrstaID = table.Column<int>(nullable: false),
                    StatusID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Takmicenja", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Takmicenja_Kategorije_KategorijaID",
                        column: x => x.KategorijaID,
                        principalTable: "Kategorije",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Takmicenja_SistemiTakmicenja_SistemID",
                        column: x => x.SistemID,
                        principalTable: "SistemiTakmicenja",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Takmicenja_StatusiTakmicenja_StatusID",
                        column: x => x.StatusID,
                        principalTable: "StatusiTakmicenja",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Takmicenja_VrsteTakmicenja_VrstaID",
                        column: x => x.VrstaID,
                        principalTable: "VrsteTakmicenja",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Utakmice",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrojUtakmice = table.Column<int>(nullable: false),
                    DatumVrijeme = table.Column<DateTime>(nullable: false),
                    Rezultat = table.Column<string>(maxLength: 10, nullable: true),
                    RundaID = table.Column<int>(nullable: false),
                    TipUtakmiceID = table.Column<int>(nullable: false),
                    StatusID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utakmice", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Utakmice_Runde_RundaID",
                        column: x => x.RundaID,
                        principalTable: "Runde",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Utakmice_StatusiUtakmice_StatusID",
                        column: x => x.StatusID,
                        principalTable: "StatusiUtakmice",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Utakmice_TipoviUtakmica_TipUtakmiceID",
                        column: x => x.TipUtakmiceID,
                        principalTable: "TipoviUtakmica",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Postovanja",
                columns: table => new
                {
                    PostivalacID = table.Column<int>(nullable: false),
                    PostovaniID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Postovanja", x => new { x.PostivalacID, x.PostovaniID });
                    table.ForeignKey(
                        name: "FK_Postovanja_Useri_PostivalacID",
                        column: x => x.PostivalacID,
                        principalTable: "Useri",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Postovanja_Useri_PostovaniID",
                        column: x => x.PostovaniID,
                        principalTable: "Useri",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Opis = table.Column<string>(nullable: true),
                    DatumKreiranja = table.Column<DateTime>(nullable: false),
                    Sadrzaj = table.Column<string>(nullable: true),
                    UserID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Reports_Useri_UserID",
                        column: x => x.UserID,
                        principalTable: "Useri",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Statistike",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrojOdigranihMeceva = table.Column<int>(nullable: false),
                    BrojSinglePobjeda = table.Column<int>(nullable: false),
                    BrojOsvojenihTurnira = table.Column<int>(nullable: false),
                    BrojOsvojenihLiga = table.Column<int>(nullable: false),
                    NajveciPobjednickiNiz = table.Column<int>(nullable: false),
                    BrojTimskihPobjeda = table.Column<int>(nullable: false),
                    AkademskaGodina = table.Column<int>(nullable: false),
                    IgracID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statistike", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Statistike_Useri_IgracID",
                        column: x => x.IgracID,
                        principalTable: "Useri",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "UseriKonverzacije",
                columns: table => new
                {
                    UserID = table.Column<int>(nullable: false),
                    KonverzacijaID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UseriKonverzacije", x => new { x.UserID, x.KonverzacijaID });
                    table.ForeignKey(
                        name: "FK_UseriKonverzacije_Konverzacije_KonverzacijaID",
                        column: x => x.KonverzacijaID,
                        principalTable: "Konverzacije",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_UseriKonverzacije_Useri_UserID",
                        column: x => x.UserID,
                        principalTable: "Useri",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "UseriUloge",
                columns: table => new
                {
                    UserID = table.Column<int>(nullable: false),
                    UlogaID = table.Column<int>(nullable: false),
                    DatumDodjele = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UseriUloge", x => new { x.UserID, x.UlogaID });
                    table.ForeignKey(
                        name: "FK_UseriUloge_Uloge_UlogaID",
                        column: x => x.UlogaID,
                        principalTable: "Uloge",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_UseriUloge_Useri_UserID",
                        column: x => x.UserID,
                        principalTable: "Useri",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Prijave",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DatumPrijave = table.Column<DateTime>(nullable: false),
                    isTim = table.Column<bool>(nullable: false),
                    Naziv = table.Column<string>(maxLength: 50, nullable: true),
                    TakmicenjeID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prijave", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Prijave_Takmicenja_TakmicenjeID",
                        column: x => x.TakmicenjeID,
                        principalTable: "Takmicenja",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "IgraciUtakmice",
                columns: table => new
                {
                    IgracID = table.Column<int>(nullable: false),
                    UtakmicaID = table.Column<int>(nullable: false),
                    ID = table.Column<int>(nullable: false),
                    PristupniElo = table.Column<int>(nullable: false),
                    OsvojeniSetovi = table.Column<int>(nullable: false),
                    TipRezultataID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IgraciUtakmice", x => new { x.IgracID, x.UtakmicaID });
                    table.ForeignKey(
                        name: "FK_IgraciUtakmice_Useri_IgracID",
                        column: x => x.IgracID,
                        principalTable: "Useri",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_IgraciUtakmice_TipoviRezultata_TipRezultataID",
                        column: x => x.TipRezultataID,
                        principalTable: "TipoviRezultata",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_IgraciUtakmice_Utakmice_UtakmicaID",
                        column: x => x.UtakmicaID,
                        principalTable: "Utakmice",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "PrijaveIgraci",
                columns: table => new
                {
                    PrijavaID = table.Column<int>(nullable: false),
                    IgracID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrijaveIgraci", x => new { x.IgracID, x.PrijavaID });
                    table.ForeignKey(
                        name: "FK_PrijaveIgraci_Useri_IgracID",
                        column: x => x.IgracID,
                        principalTable: "Useri",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_PrijaveIgraci_Prijave_PrijavaID",
                        column: x => x.PrijavaID,
                        principalTable: "Prijave",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "StanjaPrijave",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrojOdigranihMeceva = table.Column<int>(nullable: false),
                    BrojPobjeda = table.Column<int>(nullable: false),
                    BrojIzgubljenih = table.Column<int>(nullable: false),
                    BrojBodova = table.Column<int>(nullable: false),
                    PrijavaID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StanjaPrijave", x => x.ID);
                    table.ForeignKey(
                        name: "FK_StanjaPrijave_Prijave_PrijavaID",
                        column: x => x.PrijavaID,
                        principalTable: "Prijave",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeedsObjave_FeedID",
                table: "FeedsObjave",
                column: "FeedID");

            migrationBuilder.CreateIndex(
                name: "IX_FeedsObjave_ObjavaID",
                table: "FeedsObjave",
                column: "ObjavaID");

            migrationBuilder.CreateIndex(
                name: "IX_IgraciUtakmice_TipRezultataID",
                table: "IgraciUtakmice",
                column: "TipRezultataID");

            migrationBuilder.CreateIndex(
                name: "IX_IgraciUtakmice_UtakmicaID",
                table: "IgraciUtakmice",
                column: "UtakmicaID");

            migrationBuilder.CreateIndex(
                name: "IX_Poruke_KonverzacijaID",
                table: "Poruke",
                column: "KonverzacijaID");

            migrationBuilder.CreateIndex(
                name: "IX_Poruke_TipStatusaPorukeID",
                table: "Poruke",
                column: "TipStatusaPorukeID");

            migrationBuilder.CreateIndex(
                name: "IX_Postovanja_PostovaniID",
                table: "Postovanja",
                column: "PostovaniID");

            migrationBuilder.CreateIndex(
                name: "IX_Prijave_TakmicenjeID",
                table: "Prijave",
                column: "TakmicenjeID");

            migrationBuilder.CreateIndex(
                name: "IX_PrijaveIgraci_PrijavaID",
                table: "PrijaveIgraci",
                column: "PrijavaID");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_UserID",
                table: "Reports",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Runde_BracketID",
                table: "Runde",
                column: "BracketID");

            migrationBuilder.CreateIndex(
                name: "IX_StanjaPrijave_PrijavaID",
                table: "StanjaPrijave",
                column: "PrijavaID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Statistike_IgracID",
                table: "Statistike",
                column: "IgracID");

            migrationBuilder.CreateIndex(
                name: "IX_Takmicenja_KategorijaID",
                table: "Takmicenja",
                column: "KategorijaID");

            migrationBuilder.CreateIndex(
                name: "IX_Takmicenja_SistemID",
                table: "Takmicenja",
                column: "SistemID");

            migrationBuilder.CreateIndex(
                name: "IX_Takmicenja_StatusID",
                table: "Takmicenja",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_Takmicenja_VrstaID",
                table: "Takmicenja",
                column: "VrstaID");

            migrationBuilder.CreateIndex(
                name: "IX_UlogePermisije_PermisijaID",
                table: "UlogePermisije",
                column: "PermisijaID");

            migrationBuilder.CreateIndex(
                name: "IX_Useri_GradID",
                table: "Useri",
                column: "GradID");

            migrationBuilder.CreateIndex(
                name: "IX_Useri_LoginID",
                table: "Useri",
                column: "LoginID");

            migrationBuilder.CreateIndex(
                name: "IX_UseriKonverzacije_KonverzacijaID",
                table: "UseriKonverzacije",
                column: "KonverzacijaID");

            migrationBuilder.CreateIndex(
                name: "IX_UseriUloge_UlogaID",
                table: "UseriUloge",
                column: "UlogaID");

            migrationBuilder.CreateIndex(
                name: "IX_Utakmice_RundaID",
                table: "Utakmice",
                column: "RundaID");

            migrationBuilder.CreateIndex(
                name: "IX_Utakmice_StatusID",
                table: "Utakmice",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_Utakmice_TipUtakmiceID",
                table: "Utakmice",
                column: "TipUtakmiceID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeedsObjave");

            migrationBuilder.DropTable(
                name: "IgraciUtakmice");

            migrationBuilder.DropTable(
                name: "Poruke");

            migrationBuilder.DropTable(
                name: "Postovanja");

            migrationBuilder.DropTable(
                name: "PrijaveIgraci");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "StanjaPrijave");

            migrationBuilder.DropTable(
                name: "Statistike");

            migrationBuilder.DropTable(
                name: "UlogePermisije");

            migrationBuilder.DropTable(
                name: "UseriKonverzacije");

            migrationBuilder.DropTable(
                name: "UseriUloge");

            migrationBuilder.DropTable(
                name: "Feeds");

            migrationBuilder.DropTable(
                name: "Objave");

            migrationBuilder.DropTable(
                name: "TipoviRezultata");

            migrationBuilder.DropTable(
                name: "Utakmice");

            migrationBuilder.DropTable(
                name: "TipoviStatusaPoruke");

            migrationBuilder.DropTable(
                name: "Prijave");

            migrationBuilder.DropTable(
                name: "Permisije");

            migrationBuilder.DropTable(
                name: "Konverzacije");

            migrationBuilder.DropTable(
                name: "Uloge");

            migrationBuilder.DropTable(
                name: "Useri");

            migrationBuilder.DropTable(
                name: "Runde");

            migrationBuilder.DropTable(
                name: "StatusiUtakmice");

            migrationBuilder.DropTable(
                name: "TipoviUtakmica");

            migrationBuilder.DropTable(
                name: "Takmicenja");

            migrationBuilder.DropTable(
                name: "Gradovi");

            migrationBuilder.DropTable(
                name: "Logins");

            migrationBuilder.DropTable(
                name: "Brackets");

            migrationBuilder.DropTable(
                name: "Kategorije");

            migrationBuilder.DropTable(
                name: "SistemiTakmicenja");

            migrationBuilder.DropTable(
                name: "StatusiTakmicenja");

            migrationBuilder.DropTable(
                name: "VrsteTakmicenja");
        }
    }
}
