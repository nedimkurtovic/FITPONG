using Microsoft.EntityFrameworkCore.Migrations;

namespace FIT_PONG.Migrations
{
    public partial class IgracUtakmicaPKPromjenaNuliranje : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IgraciUtakmice_Igraci_IgracID",
                table: "IgraciUtakmice");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IgraciUtakmice",
                table: "IgraciUtakmice");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "IgraciUtakmice");

            migrationBuilder.AlterColumn<int>(
                name: "PristupniElo",
                table: "IgraciUtakmice",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "OsvojeniSetovi",
                table: "IgraciUtakmice",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "IgracID",
                table: "IgraciUtakmice",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "IgID",
                table: "IgraciUtakmice",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IgraciUtakmice",
                table: "IgraciUtakmice",
                column: "IgID");

            migrationBuilder.CreateIndex(
                name: "IX_IgraciUtakmice_IgracID",
                table: "IgraciUtakmice",
                column: "IgracID");

            migrationBuilder.AddForeignKey(
                name: "FK_IgraciUtakmice_Igraci_IgracID",
                table: "IgraciUtakmice",
                column: "IgracID",
                principalTable: "Igraci",
                principalColumn: "ID",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IgraciUtakmice_Igraci_IgracID",
                table: "IgraciUtakmice");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IgraciUtakmice",
                table: "IgraciUtakmice");

            migrationBuilder.DropIndex(
                name: "IX_IgraciUtakmice_IgracID",
                table: "IgraciUtakmice");

            migrationBuilder.DropColumn(
                name: "IgID",
                table: "IgraciUtakmice");

            migrationBuilder.AlterColumn<int>(
                name: "PristupniElo",
                table: "IgraciUtakmice",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "OsvojeniSetovi",
                table: "IgraciUtakmice",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IgracID",
                table: "IgraciUtakmice",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "IgraciUtakmice",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_IgraciUtakmice",
                table: "IgraciUtakmice",
                columns: new[] { "IgracID", "UtakmicaID" });

            migrationBuilder.AddForeignKey(
                name: "FK_IgraciUtakmice_Igraci_IgracID",
                table: "IgraciUtakmice",
                column: "IgracID",
                principalTable: "Igraci",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
