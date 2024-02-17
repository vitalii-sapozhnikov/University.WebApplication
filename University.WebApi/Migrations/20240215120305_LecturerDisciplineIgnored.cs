using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace University.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class LecturerDisciplineIgnored : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LecturerDisciplines");

            migrationBuilder.DropTable(
                name: "MethodologicalPublications");

            migrationBuilder.DropTable(
                name: "ScientificPublications");

            migrationBuilder.AddColumn<string>(
                name: "CloudStorageGuid",
                table: "Publications",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DOI",
                table: "Publications",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Publications",
                type: "character varying(34)",
                maxLength: 34,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "JournalDetails",
                table: "Publications",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "JournalType",
                table: "Publications",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PlanId",
                table: "Publications",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Publications",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "HeadsOfDepartments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ApplicationUserId = table.Column<string>(type: "text", nullable: false),
                    DepartmentId = table.Column<int>(type: "integer", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    MiddleName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeadsOfDepartments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HeadsOfDepartments_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HeadsOfDepartments_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "DepartmentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lecturers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AcademicTitle = table.Column<int>(type: "integer", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "text", nullable: false),
                    DepartmentId = table.Column<int>(type: "integer", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    MiddleName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lecturers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lecturers_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Lecturers_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "DepartmentId",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "IX_Publications_PlanId",
                table: "Publications",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_HeadsOfDepartments_ApplicationUserId",
                table: "HeadsOfDepartments",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_HeadsOfDepartments_DepartmentId",
                table: "HeadsOfDepartments",
                column: "DepartmentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LecturerPublication_PublicationsPublicationId",
                table: "LecturerPublication",
                column: "PublicationsPublicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Lecturers_ApplicationUserId",
                table: "Lecturers",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Lecturers_DepartmentId",
                table: "Lecturers",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Publications_Plans_PlanId",
                table: "Publications",
                column: "PlanId",
                principalTable: "Plans",
                principalColumn: "PlanId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Publications_Plans_PlanId",
                table: "Publications");

            migrationBuilder.DropTable(
                name: "HeadsOfDepartments");

            migrationBuilder.DropTable(
                name: "LecturerPublication");

            migrationBuilder.DropTable(
                name: "Lecturers");

            migrationBuilder.DropIndex(
                name: "IX_Publications_PlanId",
                table: "Publications");

            migrationBuilder.DropColumn(
                name: "CloudStorageGuid",
                table: "Publications");

            migrationBuilder.DropColumn(
                name: "DOI",
                table: "Publications");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Publications");

            migrationBuilder.DropColumn(
                name: "JournalDetails",
                table: "Publications");

            migrationBuilder.DropColumn(
                name: "JournalType",
                table: "Publications");

            migrationBuilder.DropColumn(
                name: "PlanId",
                table: "Publications");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Publications");

            migrationBuilder.CreateTable(
                name: "LecturerDisciplines",
                columns: table => new
                {
                    LecturerId = table.Column<int>(type: "integer", nullable: false),
                    DisciplineId = table.Column<int>(type: "integer", nullable: false),
                    BeginDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LecturerDisciplines", x => new { x.LecturerId, x.DisciplineId });
                    table.ForeignKey(
                        name: "FK_LecturerDisciplines_Disciplines_DisciplineId",
                        column: x => x.DisciplineId,
                        principalTable: "Disciplines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MethodologicalPublications",
                columns: table => new
                {
                    PublicationId = table.Column<int>(type: "integer", nullable: false),
                    PlanId = table.Column<int>(type: "integer", nullable: true),
                    CloudStorageGuid = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_LecturerDisciplines_DisciplineId",
                table: "LecturerDisciplines",
                column: "DisciplineId");

            migrationBuilder.CreateIndex(
                name: "IX_MethodologicalPublications_PlanId",
                table: "MethodologicalPublications",
                column: "PlanId");
        }
    }
}
