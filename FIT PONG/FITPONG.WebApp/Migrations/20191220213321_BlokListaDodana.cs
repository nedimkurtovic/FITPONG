using Microsoft.EntityFrameworkCore.Migrations;

namespace FIT_PONG.Migrations
{
    public partial class BlokListaDodana : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlokListe",
                columns: table => new
                {
                    IgracID = table.Column<int>(nullable: false),
                    TakmicenjeID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlokListe", x => new { x.IgracID, x.TakmicenjeID });
                    table.ForeignKey(
                        name: "FK_BlokListe_Igraci_IgracID",
                        column: x => x.IgracID,
                        principalTable: "Igraci",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlokListe_Takmicenja_TakmicenjeID",
                        column: x => x.TakmicenjeID,
                        principalTable: "Takmicenja",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlokListe_TakmicenjeID",
                table: "BlokListe",
                column: "TakmicenjeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlokListe");
        }
    }
}
