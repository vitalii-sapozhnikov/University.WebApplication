using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace University.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class PersonPublicationRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LecturerPublication");

            migrationBuilder.CreateTable(
                name: "PersonPublication",
                columns: table => new
                {
                    AuthorsId = table.Column<int>(type: "integer", nullable: false),
                    PublicationsPublicationId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonPublication", x => new { x.AuthorsId, x.PublicationsPublicationId });
                    table.ForeignKey(
                        name: "FK_PersonPublication_Person_AuthorsId",
                        column: x => x.AuthorsId,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonPublication_Publications_PublicationsPublicationId",
                        column: x => x.PublicationsPublicationId,
                        principalTable: "Publications",
                        principalColumn: "PublicationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonPublication_PublicationsPublicationId",
                table: "PersonPublication",
                column: "PublicationsPublicationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonPublication");

            migrationBuilder.CreateTable(
                name: "LecturerPublication",
                columns: table => new
                {
                    LecturersId = table.Column<int>(type: "integer", nullable: false),
                    PublicationsPublicationId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LecturerPublication", x => new { x.LecturersId, x.PublicationsPublicationId });
                    table.ForeignKey(
                        name: "FK_LecturerPublication_Lecturers_LecturersId",
                        column: x => x.LecturersId,
                        principalTable: "Lecturers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LecturerPublication_Publications_PublicationsPublicationId",
                        column: x => x.PublicationsPublicationId,
                        principalTable: "Publications",
                        principalColumn: "PublicationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LecturerPublication_PublicationsPublicationId",
                table: "LecturerPublication",
                column: "PublicationsPublicationId");
        }
    }
}
