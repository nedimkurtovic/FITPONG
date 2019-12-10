using Microsoft.EntityFrameworkCore.Migrations;

namespace FIT_PONG.Migrations
{
    public partial class TakmicenjeFeedovi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FeedID",
                table: "Takmicenja",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Takmicenja_FeedID",
                table: "Takmicenja",
                column: "FeedID");

            migrationBuilder.AddForeignKey(
                name: "FK_Takmicenja_Feeds_FeedID",
                table: "Takmicenja",
                column: "FeedID",
                principalTable: "Feeds",
                principalColumn: "ID",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Takmicenja_Feeds_FeedID",
                table: "Takmicenja");

            migrationBuilder.DropIndex(
                name: "IX_Takmicenja_FeedID",
                table: "Takmicenja");

            migrationBuilder.DropColumn(
                name: "FeedID",
                table: "Takmicenja");
        }
    }
}
