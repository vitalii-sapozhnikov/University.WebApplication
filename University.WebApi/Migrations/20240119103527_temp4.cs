using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace University.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class temp4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Publications_AspNetUsers_userId",
                table: "Publications");

            migrationBuilder.DropIndex(
                name: "IX_Publications_userId",
                table: "Publications");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "Publications");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
