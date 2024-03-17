using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace University.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class ScientificPublicationsIgnored : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScientificArticles");

            migrationBuilder.DropTable(
                name: "ScientificDissertations");

            migrationBuilder.DropTable(
                name: "ScientificMonographs");

            migrationBuilder.DropTable(
                name: "ScientificPatents");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ScientificArticles",
                columns: table => new
                {
                    PublicationId = table.Column<int>(type: "integer", nullable: false),
                    DOI = table.Column<string>(type: "text", nullable: false),
                    JournalDetails = table.Column<string>(type: "text", nullable: false),
                    JournalType = table.Column<int>(type: "integer", nullable: false),
                    UDC = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScientificArticles", x => x.PublicationId);
                    table.ForeignKey(
                        name: "FK_ScientificArticles_Publications_PublicationId",
                        column: x => x.PublicationId,
                        principalTable: "Publications",
                        principalColumn: "PublicationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScientificDissertations",
                columns: table => new
                {
                    PublicationId = table.Column<int>(type: "integer", nullable: false),
                    DOI = table.Column<string>(type: "text", nullable: false),
                    DissertationType = table.Column<int>(type: "integer", nullable: false),
                    EducationalInstitution = table.Column<string>(type: "text", nullable: false),
                    UDC = table.Column<string>(type: "text", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "ScientificMonographs",
                columns: table => new
                {
                    PublicationId = table.Column<int>(type: "integer", nullable: false),
                    ISBN = table.Column<string>(type: "text", nullable: false),
                    Publisher = table.Column<string>(type: "text", nullable: false),
                    UDC = table.Column<string>(type: "text", nullable: false),
                    URL = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScientificMonographs", x => x.PublicationId);
                    table.ForeignKey(
                        name: "FK_ScientificMonographs_Publications_PublicationId",
                        column: x => x.PublicationId,
                        principalTable: "Publications",
                        principalColumn: "PublicationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScientificPatents",
                columns: table => new
                {
                    PublicationId = table.Column<int>(type: "integer", nullable: false),
                    Issuer = table.Column<string>(type: "text", nullable: false),
                    PatentNo = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScientificPatents", x => x.PublicationId);
                    table.ForeignKey(
                        name: "FK_ScientificPatents_Publications_PublicationId",
                        column: x => x.PublicationId,
                        principalTable: "Publications",
                        principalColumn: "PublicationId",
                        onDelete: ReferentialAction.Cascade);
                });
        }
    }
}
