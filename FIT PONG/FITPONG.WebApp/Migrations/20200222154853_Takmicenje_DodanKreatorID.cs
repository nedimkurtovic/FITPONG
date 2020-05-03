using Microsoft.EntityFrameworkCore.Migrations;

namespace FIT_PONG.Migrations
{
    public partial class Takmicenje_DodanKreatorID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "KreatorID",
                table: "Takmicenja",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Takmicenja_KreatorID",
                table: "Takmicenja",
                column: "KreatorID");

            migrationBuilder.AddForeignKey(
                name: "FK_Takmicenja_Igraci_KreatorID",
                table: "Takmicenja",
                column: "KreatorID",
                principalTable: "Igraci",
                principalColumn: "ID",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Takmicenja_Igraci_KreatorID",
                table: "Takmicenja");

            migrationBuilder.DropIndex(
                name: "IX_Takmicenja_KreatorID",
                table: "Takmicenja");

            migrationBuilder.DropColumn(
                name: "KreatorID",
                table: "Takmicenja");
        }
    }
}
