using Microsoft.EntityFrameworkCore.Migrations;

namespace FIT_PONG.Migrations
{
    public partial class TipRezultataUtakmiceNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IgraciUtakmice_TipoviRezultata_TipRezultataID",
                table: "IgraciUtakmice");

            migrationBuilder.AlterColumn<int>(
                name: "TipRezultataID",
                table: "IgraciUtakmice",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_IgraciUtakmice_TipoviRezultata_TipRezultataID",
                table: "IgraciUtakmice",
                column: "TipRezultataID",
                principalTable: "TipoviRezultata",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IgraciUtakmice_TipoviRezultata_TipRezultataID",
                table: "IgraciUtakmice");

            migrationBuilder.AlterColumn<int>(
                name: "TipRezultataID",
                table: "IgraciUtakmice",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_IgraciUtakmice_TipoviRezultata_TipRezultataID",
                table: "IgraciUtakmice",
                column: "TipRezultataID",
                principalTable: "TipoviRezultata",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
