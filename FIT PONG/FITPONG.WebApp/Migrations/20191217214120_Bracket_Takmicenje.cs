using Microsoft.EntityFrameworkCore.Migrations;

namespace FIT_PONG.Migrations
{
    public partial class Bracket_Takmicenje : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Naziv",
                table: "Prijave",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Visina",
                table: "Igraci",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<string>(
                name: "PrikaznoIme",
                table: "Igraci",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TakmicenjeID",
                table: "Brackets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Brackets_TakmicenjeID",
                table: "Brackets",
                column: "TakmicenjeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Brackets_Takmicenja_TakmicenjeID",
                table: "Brackets",
                column: "TakmicenjeID",
                principalTable: "Takmicenja",
                principalColumn: "ID",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Brackets_Takmicenja_TakmicenjeID",
                table: "Brackets");

            migrationBuilder.DropIndex(
                name: "IX_Brackets_TakmicenjeID",
                table: "Brackets");

            migrationBuilder.DropColumn(
                name: "TakmicenjeID",
                table: "Brackets");

            migrationBuilder.AlterColumn<string>(
                name: "Naziv",
                table: "Prijave",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<double>(
                name: "Visina",
                table: "Igraci",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PrikaznoIme",
                table: "Igraci",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50);
        }
    }
}
