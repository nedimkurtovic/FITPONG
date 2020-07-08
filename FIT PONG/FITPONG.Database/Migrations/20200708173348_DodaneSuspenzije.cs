using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FIT_PONG.Migrations
{
    public partial class DodaneSuspenzije : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VrsteSuspenzije",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Opis = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VrsteSuspenzije", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Suspenzije",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IgracID = table.Column<int>(nullable: false),
                    VrstaSuspenzijeID = table.Column<int>(nullable: false),
                    DatumPocetka = table.Column<DateTime>(nullable: false),
                    DatumZavrsetka = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suspenzije", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Suspenzije_Igraci_IgracID",
                        column: x => x.IgracID,
                        principalTable: "Igraci",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Suspenzije_VrsteSuspenzije_VrstaSuspenzijeID",
                        column: x => x.VrstaSuspenzijeID,
                        principalTable: "VrsteSuspenzije",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Suspenzije_IgracID",
                table: "Suspenzije",
                column: "IgracID");

            migrationBuilder.CreateIndex(
                name: "IX_Suspenzije_VrstaSuspenzijeID",
                table: "Suspenzije",
                column: "VrstaSuspenzijeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Suspenzije");

            migrationBuilder.DropTable(
                name: "VrsteSuspenzije");
        }
    }
}
