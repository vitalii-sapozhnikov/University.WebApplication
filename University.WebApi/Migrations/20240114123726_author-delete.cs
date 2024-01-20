using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace University.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class authordelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthorPublication_Authors_AuthorsAuthorId",
                table: "AuthorPublication");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AuthorPublication",
                table: "AuthorPublication");

            migrationBuilder.DropColumn(
                name: "AuthorsAuthorId",
                table: "AuthorPublication");

            migrationBuilder.AddColumn<string>(
                name: "AuthorsId",
                table: "AuthorPublication",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "character varying(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AuthorPublication",
                table: "AuthorPublication",
                columns: new[] { "AuthorsId", "PublicationsPublicationId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorPublication_AspNetUsers_AuthorsId",
                table: "AuthorPublication",
                column: "AuthorsId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthorPublication_AspNetUsers_AuthorsId",
                table: "AuthorPublication");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AuthorPublication",
                table: "AuthorPublication");

            migrationBuilder.DropColumn(
                name: "AuthorsId",
                table: "AuthorPublication");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "AuthorsAuthorId",
                table: "AuthorPublication",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AuthorPublication",
                table: "AuthorPublication",
                columns: new[] { "AuthorsAuthorId", "PublicationsPublicationId" });

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

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorPublication_Authors_AuthorsAuthorId",
                table: "AuthorPublication",
                column: "AuthorsAuthorId",
                principalTable: "Authors",
                principalColumn: "AuthorId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
