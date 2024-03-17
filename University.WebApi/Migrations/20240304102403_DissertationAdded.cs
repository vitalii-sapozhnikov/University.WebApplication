using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace University.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class DissertationAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ScientificDissertations",
                columns: table => new
                {
                    PublicationId = table.Column<int>(type: "integer", nullable: false),
                    EducationalInstitution = table.Column<string>(type: "text", nullable: false),
                    DissertationType = table.Column<int>(type: "integer", nullable: false),
                    UDC = table.Column<string>(type: "text", nullable: false),
                    DOI = table.Column<string>(type: "text", nullable: false),
                    URL = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScientificDissertations", x => x.PublicationId);
                    table.ForeignKey(
                        name: "FK_ScientificDissertations_Publications_PublicationId",
                        column: x => x.PublicationId,
                        principalTable: "Publications",
                        principalColumn: "PublicationId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScientificDissertations");
        }
    }
}
