using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FIT_PONG.Migrations
{
    public partial class IzbacivanjeViskaTabela : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Poruke");

            migrationBuilder.DropTable(
                name: "UseriKonverzacije");

            migrationBuilder.DropTable(
                name: "TipoviStatusaPoruke");

            migrationBuilder.DropTable(
                name: "Konverzacije");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Konverzacije",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DatumKreiranja = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Konverzacije", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TipoviStatusaPoruke",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Naziv = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoviStatusaPoruke", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "UseriKonverzacije",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false),
                    KonverzacijaID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UseriKonverzacije", x => new { x.UserID, x.KonverzacijaID });
                    table.ForeignKey(
                        name: "FK_UseriKonverzacije_Konverzacije_KonverzacijaID",
                        column: x => x.KonverzacijaID,
                        principalTable: "Konverzacije",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UseriKonverzacije_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Poruke",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DatumVrijeme = table.Column<DateTime>(type: "datetime2", nullable: false),
                    KonverzacijaID = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TipStatusaPorukeID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Poruke", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Poruke_Konverzacije_KonverzacijaID",
                        column: x => x.KonverzacijaID,
                        principalTable: "Konverzacije",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Poruke_TipoviStatusaPoruke_TipStatusaPorukeID",
                        column: x => x.TipStatusaPorukeID,
                        principalTable: "TipoviStatusaPoruke",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Poruke_KonverzacijaID",
                table: "Poruke",
                column: "KonverzacijaID");

            migrationBuilder.CreateIndex(
                name: "IX_Poruke_TipStatusaPorukeID",
                table: "Poruke",
                column: "TipStatusaPorukeID");

            migrationBuilder.CreateIndex(
                name: "IX_UseriKonverzacije_KonverzacijaID",
                table: "UseriKonverzacije",
                column: "KonverzacijaID");
        }
    }
}
