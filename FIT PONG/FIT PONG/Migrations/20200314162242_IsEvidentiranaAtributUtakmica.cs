using Microsoft.EntityFrameworkCore.Migrations;

namespace FIT_PONG.Migrations
{
    public partial class IsEvidentiranaAtributUtakmica : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEvidentirana",
                table: "Utakmice",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEvidentirana",
                table: "Utakmice");
        }
    }
}
