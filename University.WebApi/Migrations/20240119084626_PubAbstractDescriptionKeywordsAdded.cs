using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace University.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class PubAbstractDescriptionKeywordsAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Abstract",
                table: "Publications",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Publications",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string[]>(
                name: "Keywords",
                table: "Publications",
                type: "text[]",
                nullable: false,
                defaultValue: new string[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Abstract",
                table: "Publications");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Publications");

            migrationBuilder.DropColumn(
                name: "Keywords",
                table: "Publications");
        }
    }
}
