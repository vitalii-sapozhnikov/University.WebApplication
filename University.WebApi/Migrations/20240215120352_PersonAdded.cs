using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace University.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class PersonAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HeadsOfDepartments_AspNetUsers_ApplicationUserId",
                table: "HeadsOfDepartments");

            migrationBuilder.DropForeignKey(
                name: "FK_Lecturers_AspNetUsers_ApplicationUserId",
                table: "Lecturers");

            migrationBuilder.DropForeignKey(
                name: "FK_Publications_Plans_PlanId",
                table: "Publications");

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

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Lecturers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Lecturers");

            migrationBuilder.DropColumn(
                name: "MiddleName",
                table: "Lecturers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "HeadsOfDepartments");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "HeadsOfDepartments");

            migrationBuilder.DropColumn(
                name: "MiddleName",
                table: "HeadsOfDepartments");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Lecturers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Lecturers",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "HeadsOfDepartments",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "HeadsOfDepartments",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

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
                    table.ForeignKey(
                        name: "FK_LecturerDisciplines_Lecturers_LecturerId",
                        column: x => x.LecturerId,
                        principalTable: "Lecturers",
                        principalColumn: "Id",
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
                name: "Person",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    MiddleName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.Id);
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
                name: "IX_LecturerDisciplines_DisciplineId",
                table: "LecturerDisciplines",
                column: "DisciplineId");

            migrationBuilder.CreateIndex(
                name: "IX_MethodologicalPublications_PlanId",
                table: "MethodologicalPublications",
                column: "PlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_HeadsOfDepartments_AspNetUsers_ApplicationUserId",
                table: "HeadsOfDepartments",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HeadsOfDepartments_Person_Id",
                table: "HeadsOfDepartments",
                column: "Id",
                principalTable: "Person",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lecturers_AspNetUsers_ApplicationUserId",
                table: "Lecturers",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lecturers_Person_Id",
                table: "Lecturers",
                column: "Id",
                principalTable: "Person",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HeadsOfDepartments_AspNetUsers_ApplicationUserId",
                table: "HeadsOfDepartments");

            migrationBuilder.DropForeignKey(
                name: "FK_HeadsOfDepartments_Person_Id",
                table: "HeadsOfDepartments");

            migrationBuilder.DropForeignKey(
                name: "FK_Lecturers_AspNetUsers_ApplicationUserId",
                table: "Lecturers");

            migrationBuilder.DropForeignKey(
                name: "FK_Lecturers_Person_Id",
                table: "Lecturers");

            migrationBuilder.DropTable(
                name: "LecturerDisciplines");

            migrationBuilder.DropTable(
                name: "MethodologicalPublications");

            migrationBuilder.DropTable(
                name: "Person");

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

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Lecturers",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Lecturers",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Lecturers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Lecturers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                table: "Lecturers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "HeadsOfDepartments",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "HeadsOfDepartments",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "HeadsOfDepartments",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "HeadsOfDepartments",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                table: "HeadsOfDepartments",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Publications_PlanId",
                table: "Publications",
                column: "PlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_HeadsOfDepartments_AspNetUsers_ApplicationUserId",
                table: "HeadsOfDepartments",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lecturers_AspNetUsers_ApplicationUserId",
                table: "Lecturers",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Publications_Plans_PlanId",
                table: "Publications",
                column: "PlanId",
                principalTable: "Plans",
                principalColumn: "PlanId",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
