using Microsoft.EntityFrameworkCore.Migrations;

namespace FIT_PONG.Migrations
{
    public partial class IgraciTabelaTPT : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IgraciUtakmice_Useri_IgracID",
                table: "IgraciUtakmice");

            migrationBuilder.DropForeignKey(
                name: "FK_Postovanja_Useri_PostivalacID",
                table: "Postovanja");

            migrationBuilder.DropForeignKey(
                name: "FK_Postovanja_Useri_PostovaniID",
                table: "Postovanja");

            migrationBuilder.DropForeignKey(
                name: "FK_PrijaveIgraci_Useri_IgracID",
                table: "PrijaveIgraci");

            migrationBuilder.DropForeignKey(
                name: "FK_Statistike_Useri_IgracID",
                table: "Statistike");

            migrationBuilder.DropColumn(
                name: "BrojPosjetaNaProfil",
                table: "Useri");

            migrationBuilder.DropColumn(
                name: "JacaRuka",
                table: "Useri");

            migrationBuilder.DropColumn(
                name: "PrikaznoIme",
                table: "Useri");

            migrationBuilder.DropColumn(
                name: "ProfileImagePath",
                table: "Useri");

            migrationBuilder.DropColumn(
                name: "Visina",
                table: "Useri");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Useri");

            migrationBuilder.CreateTable(
                name: "Igraci",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false),
                    PrikaznoIme = table.Column<string>(maxLength: 50, nullable: true),
                    JacaRuka = table.Column<string>(maxLength: 8, nullable: true),
                    Visina = table.Column<double>(nullable: true),
                    BrojPosjetaNaProfil = table.Column<int>(nullable: false),
                    ProfileImagePath = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Igraci", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Igraci_Useri_ID",
                        column: x => x.ID,
                        principalTable: "Useri",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_IgraciUtakmice_Igraci_IgracID",
                table: "IgraciUtakmice",
                column: "IgracID",
                principalTable: "Igraci",
                principalColumn: "ID",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Postovanja_Igraci_PostivalacID",
                table: "Postovanja",
                column: "PostivalacID",
                principalTable: "Igraci",
                principalColumn: "ID",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Postovanja_Igraci_PostovaniID",
                table: "Postovanja",
                column: "PostovaniID",
                principalTable: "Igraci",
                principalColumn: "ID",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_PrijaveIgraci_Igraci_IgracID",
                table: "PrijaveIgraci",
                column: "IgracID",
                principalTable: "Igraci",
                principalColumn: "ID",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Statistike_Igraci_IgracID",
                table: "Statistike",
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

            migrationBuilder.DropForeignKey(
                name: "FK_Postovanja_Igraci_PostivalacID",
                table: "Postovanja");

            migrationBuilder.DropForeignKey(
                name: "FK_Postovanja_Igraci_PostovaniID",
                table: "Postovanja");

            migrationBuilder.DropForeignKey(
                name: "FK_PrijaveIgraci_Igraci_IgracID",
                table: "PrijaveIgraci");

            migrationBuilder.DropForeignKey(
                name: "FK_Statistike_Igraci_IgracID",
                table: "Statistike");

            migrationBuilder.DropTable(
                name: "Igraci");

            migrationBuilder.AddColumn<int>(
                name: "BrojPosjetaNaProfil",
                table: "Useri",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JacaRuka",
                table: "Useri",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrikaznoIme",
                table: "Useri",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfileImagePath",
                table: "Useri",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Visina",
                table: "Useri",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Useri",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_IgraciUtakmice_Useri_IgracID",
                table: "IgraciUtakmice",
                column: "IgracID",
                principalTable: "Useri",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Postovanja_Useri_PostivalacID",
                table: "Postovanja",
                column: "PostivalacID",
                principalTable: "Useri",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Postovanja_Useri_PostovaniID",
                table: "Postovanja",
                column: "PostovaniID",
                principalTable: "Useri",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PrijaveIgraci_Useri_IgracID",
                table: "PrijaveIgraci",
                column: "IgracID",
                principalTable: "Useri",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Statistike_Useri_IgracID",
                table: "Statistike",
                column: "IgracID",
                principalTable: "Useri",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
