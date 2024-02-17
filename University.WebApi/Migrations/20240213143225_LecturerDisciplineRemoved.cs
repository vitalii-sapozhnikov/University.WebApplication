using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace University.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class LecturerDisciplineRemoved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DisciplineLecturer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateIndex(
                name: "IX_DisciplineLecturer_LecturersLecturerId",
                table: "DisciplineLecturer",
                column: "LecturersLecturerId");
        }
    }
}
