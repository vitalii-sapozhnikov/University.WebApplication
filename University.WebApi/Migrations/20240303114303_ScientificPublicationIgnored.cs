using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace University.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class ScientificPublicationIgnored : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScientificPublications");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ScientificPublications",
                columns: table => new
                {
                    PublicationId = table.Column<int>(type: "integer", nullable: false),
                    DOI = table.Column<string>(type: "text", nullable: false),
                    JournalDetails = table.Column<string>(type: "text", nullable: false),
                    JournalType = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScientificPublications", x => x.PublicationId);
                    table.ForeignKey(
                        name: "FK_ScientificPublications_Publications_PublicationId",
                        column: x => x.PublicationId,
                        principalTable: "Publications",
                        principalColumn: "PublicationId",
                        onDelete: ReferentialAction.Cascade);
                });
        }
    }
}
