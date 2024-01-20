using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace University.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class temp3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Publications_PublicationId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PublicationId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PublicationId",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    AuthorId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.AuthorId);
                });

            migrationBuilder.CreateTable(
                name: "AuthorPublication",
                columns: table => new
                {
                    AuthorsAuthorId = table.Column<int>(type: "integer", nullable: false),
                    PublicationsPublicationId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorPublication", x => new { x.AuthorsAuthorId, x.PublicationsPublicationId });
                    table.ForeignKey(
                        name: "FK_AuthorPublication_Authors_AuthorsAuthorId",
                        column: x => x.AuthorsAuthorId,
                        principalTable: "Authors",
                        principalColumn: "AuthorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthorPublication_Publications_PublicationsPublicationId",
                        column: x => x.PublicationsPublicationId,
                        principalTable: "Publications",
                        principalColumn: "PublicationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthorPublication_PublicationsPublicationId",
                table: "AuthorPublication",
                column: "PublicationsPublicationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorPublication");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "character varying(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PublicationId",
                table: "AspNetUsers",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PublicationId",
                table: "AspNetUsers",
                column: "PublicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Publications_PublicationId",
                table: "AspNetUsers",
                column: "PublicationId",
                principalTable: "Publications",
                principalColumn: "PublicationId");
        }
    }
}
