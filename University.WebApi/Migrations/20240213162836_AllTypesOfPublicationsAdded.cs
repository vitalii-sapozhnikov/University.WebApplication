using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace University.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class AllTypesOfPublicationsAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Publications",
                columns: table => new
                {
                    PublicationId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    PublicationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Abstract = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Keywords = table.Column<string[]>(type: "text[]", nullable: true),
                    isPublished = table.Column<bool>(type: "boolean", nullable: false),
                    Volume = table.Column<int>(type: "integer", nullable: true),
                    Language = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publications", x => x.PublicationId);
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

            migrationBuilder.CreateTable(
                name: "LecturerPublication",
                columns: table => new
                {
                    LecturersLecturerId = table.Column<int>(type: "integer", nullable: false),
                    PublicationsPublicationId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LecturerPublication", x => new { x.LecturersLecturerId, x.PublicationsPublicationId });
                    table.ForeignKey(
                        name: "FK_LecturerPublication_Lecturers_LecturersLecturerId",
                        column: x => x.LecturersLecturerId,
                        principalTable: "Lecturers",
                        principalColumn: "LecturerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LecturerPublication_Publications_PublicationsPublicationId",
                        column: x => x.PublicationsPublicationId,
                        principalTable: "Publications",
                        principalColumn: "PublicationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MethodologicalPublications",
                columns: table => new
                {
                    PublicationId = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: true),
                    CloudStorageGuid = table.Column<string>(type: "text", nullable: true),
                    PlanId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MethodologicalPublications", x => x.PublicationId);
                    table.ForeignKey(
                        name: "FK_MethodologicalPublications_Plans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "Plans",
                        principalColumn: "PlanId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_MethodologicalPublications_Publications_PublicationId",
                        column: x => x.PublicationId,
                        principalTable: "Publications",
                        principalColumn: "PublicationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScientificPublications",
                columns: table => new
                {
                    PublicationId = table.Column<int>(type: "integer", nullable: false),
                    JournalType = table.Column<int>(type: "integer", nullable: false),
                    JournalDetails = table.Column<string>(type: "text", nullable: false),
                    DOI = table.Column<string>(type: "text", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_DisciplinePublication_PublicationsPublicationId",
                table: "DisciplinePublication",
                column: "PublicationsPublicationId");

            migrationBuilder.CreateIndex(
                name: "IX_LecturerPublication_PublicationsPublicationId",
                table: "LecturerPublication",
                column: "PublicationsPublicationId");

            migrationBuilder.CreateIndex(
                name: "IX_MethodologicalPublications_PlanId",
                table: "MethodologicalPublications",
                column: "PlanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DisciplinePublication");

            migrationBuilder.DropTable(
                name: "LecturerPublication");

            migrationBuilder.DropTable(
                name: "MethodologicalPublications");

            migrationBuilder.DropTable(
                name: "ScientificPublications");

            migrationBuilder.DropTable(
                name: "Publications");
        }
    }
}
