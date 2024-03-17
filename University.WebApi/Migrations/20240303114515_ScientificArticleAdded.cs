using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace University.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class ScientificArticleAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ScientificArticles",
                columns: table => new
                {
                    PublicationId = table.Column<int>(type: "integer", nullable: false),
                    JournalType = table.Column<int>(type: "integer", nullable: false),
                    JournalDetails = table.Column<string>(type: "text", nullable: false),
                    DOI = table.Column<string>(type: "text", nullable: false)
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScientificArticles");
        }
    }
}
