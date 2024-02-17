using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace University.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class DisciplineManyToManyFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lecturers_Disciplines_DisciplineId",
                table: "Lecturers");

            migrationBuilder.DropForeignKey(
                name: "FK_Publications_Disciplines_DisciplineId",
                table: "Publications");

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

            migrationBuilder.CreateTable(
                name: "DisciplineLecturer",
                columns: table => new
                {
                    DisciplinesId = table.Column<int>(type: "integer", nullable: false),
                    LecturersLecturerId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisciplineLecturer", x => new { x.DisciplinesId, x.LecturersLecturerId });
                    table.ForeignKey(
                        name: "FK_DisciplineLecturer_Disciplines_DisciplinesId",
                        column: x => x.DisciplinesId,
                        principalTable: "Disciplines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DisciplineLecturer_Lecturers_LecturersLecturerId",
                        column: x => x.LecturersLecturerId,
                        principalTable: "Lecturers",
                        principalColumn: "LecturerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DisciplinePublication",
                columns: table => new
                {
                    DisciplinesId = table.Column<int>(type: "integer", nullable: false),
                    PublicationsPublicationId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisciplinePublication", x => new { x.DisciplinesId, x.PublicationsPublicationId });
                    table.ForeignKey(
                        name: "FK_DisciplinePublication_Disciplines_DisciplinesId",
                        column: x => x.DisciplinesId,
                        principalTable: "Disciplines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DisciplinePublication_Publications_PublicationsPublicationId",
                        column: x => x.PublicationsPublicationId,
                        principalTable: "Publications",
                        principalColumn: "PublicationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DisciplineLecturer_LecturersLecturerId",
                table: "DisciplineLecturer",
                column: "LecturersLecturerId");

            migrationBuilder.CreateIndex(
                name: "IX_DisciplinePublication_PublicationsPublicationId",
                table: "DisciplinePublication",
                column: "PublicationsPublicationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DisciplineLecturer");

            migrationBuilder.DropTable(
                name: "DisciplinePublication");

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
    }
}
