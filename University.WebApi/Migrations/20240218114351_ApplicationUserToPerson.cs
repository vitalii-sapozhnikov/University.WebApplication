using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace University.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class ApplicationUserToPerson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HeadsOfDepartments_AspNetUsers_ApplicationUserId",
                table: "HeadsOfDepartments");

            migrationBuilder.DropForeignKey(
                name: "FK_Lecturers_AspNetUsers_ApplicationUserId",
                table: "Lecturers");

            migrationBuilder.DropIndex(
                name: "IX_Lecturers_ApplicationUserId",
                table: "Lecturers");

            migrationBuilder.DropIndex(
                name: "IX_HeadsOfDepartments_ApplicationUserId",
                table: "HeadsOfDepartments");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Lecturers");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "HeadsOfDepartments");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Person",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Person_ApplicationUserId",
                table: "Person",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Person_AspNetUsers_ApplicationUserId",
                table: "Person",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Person_AspNetUsers_ApplicationUserId",
                table: "Person");

            migrationBuilder.DropIndex(
                name: "IX_Person_ApplicationUserId",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Person");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Lecturers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "HeadsOfDepartments",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lecturers_ApplicationUserId",
                table: "Lecturers",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_HeadsOfDepartments_ApplicationUserId",
                table: "HeadsOfDepartments",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_HeadsOfDepartments_AspNetUsers_ApplicationUserId",
                table: "HeadsOfDepartments",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lecturers_AspNetUsers_ApplicationUserId",
                table: "Lecturers",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
