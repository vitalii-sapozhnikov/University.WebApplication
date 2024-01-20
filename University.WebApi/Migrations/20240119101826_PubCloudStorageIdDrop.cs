using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace University.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class PubCloudStorageIdDrop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CloudStorageGuid",
                table: "Publications",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CloudStorageGuid",
                table: "Publications");

            
        }
    }
}
