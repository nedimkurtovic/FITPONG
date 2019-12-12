using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FIT_PONG.Migrations
{
    public partial class ReportAttachmenti : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Useri_UserID",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_UserID",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Reports");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Reports",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Attachments",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Path = table.Column<string>(nullable: false),
                    DatumUnosa = table.Column<DateTime>(nullable: false),
                    ReportID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Attachments_Reports_ReportID",
                        column: x => x.ReportID,
                        principalTable: "Reports",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_ReportID",
                table: "Attachments",
                column: "ReportID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attachments");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Reports");

            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_UserID",
                table: "Reports",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Useri_UserID",
                table: "Reports",
                column: "UserID",
                principalTable: "Useri",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
