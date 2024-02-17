using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace University.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class DisciplineAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DisciplineId",
                table: "Publications",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DisciplineId",
                table: "Lecturers",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Disciplines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Disciplines", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Disciplines",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Організація бази даних" },
                    { 2, "Інженерія програмного забезпечення" },
                    { 3, "Введення в систему підтримки прийняття рішень" },
                    { 4, "Технологія тестування програмного забезпечення" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Publications_DisciplineId",
                table: "Publications",
                column: "DisciplineId");

            migrationBuilder.CreateIndex(
                name: "IX_Lecturers_DisciplineId",
                table: "Lecturers",
                column: "DisciplineId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lecturers_Disciplines_DisciplineId",
                table: "Lecturers",
                column: "DisciplineId",
                principalTable: "Disciplines",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Publications_Disciplines_DisciplineId",
                table: "Publications",
                column: "DisciplineId",
                principalTable: "Disciplines",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lecturers_Disciplines_DisciplineId",
                table: "Lecturers");

            migrationBuilder.DropForeignKey(
                name: "FK_Publications_Disciplines_DisciplineId",
                table: "Publications");

            migrationBuilder.DropTable(
                name: "Disciplines");

            migrationBuilder.DropIndex(
                name: "IX_Publications_DisciplineId",
                table: "Publications");

            migrationBuilder.DropIndex(
                name: "IX_Lecturers_DisciplineId",
                table: "Lecturers");

            migrationBuilder.DropColumn(
                name: "DisciplineId",
                table: "Publications");

            migrationBuilder.DropColumn(
                name: "DisciplineId",
                table: "Lecturers");
        }
    }
}
