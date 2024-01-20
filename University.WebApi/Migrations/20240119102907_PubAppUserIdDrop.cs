using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace University.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class PubAppUserIdDrop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Publications");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Publications",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Publications_UserId",
                table: "Publications",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Publications_AspNetUsers_UserId",
                table: "Publications",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Publications_AspNetUsers_UserId",
                table: "Publications");

            migrationBuilder.DropIndex(
                name: "IX_Publications_UserId",
                table: "Publications");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Publications");
        }
    }
}
