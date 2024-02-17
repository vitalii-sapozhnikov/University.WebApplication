using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace University.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class PlanAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Department",
                keyColumn: "DepartmentId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Department",
                keyColumn: "DepartmentId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Department",
                keyColumn: "DepartmentId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Department",
                keyColumn: "DepartmentId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Department",
                keyColumn: "DepartmentId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Department",
                keyColumn: "DepartmentId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Department",
                keyColumn: "DepartmentId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Department",
                keyColumn: "DepartmentId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Department",
                keyColumn: "DepartmentId",
                keyValue: 9);

            migrationBuilder.AlterColumn<DateTime>(
                name: "PublicationDate",
                table: "Publications",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<string[]>(
                name: "Keywords",
                table: "Publications",
                type: "text[]",
                nullable: true,
                oldClrType: typeof(string[]),
                oldType: "text[]");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Publications",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "CloudStorageGuid",
                table: "Publications",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Abstract",
                table: "Publications",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "Language",
                table: "Publications",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PlanId",
                table: "Publications",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Volume",
                table: "Publications",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isPublished",
                table: "Publications",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Plan",
                columns: table => new
                {
                    PlanId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    DepartmentId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plan", x => x.PlanId);
                    table.ForeignKey(
                        name: "FK_Plan_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "DepartmentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Publications_PlanId",
                table: "Publications",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Plan_DepartmentId",
                table: "Plan",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Publications_Plan_PlanId",
                table: "Publications",
                column: "PlanId",
                principalTable: "Plan",
                principalColumn: "PlanId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Publications_Plan_PlanId",
                table: "Publications");

            migrationBuilder.DropTable(
                name: "Plan");

            migrationBuilder.DropIndex(
                name: "IX_Publications_PlanId",
                table: "Publications");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "Publications");

            migrationBuilder.DropColumn(
                name: "PlanId",
                table: "Publications");

            migrationBuilder.DropColumn(
                name: "Volume",
                table: "Publications");

            migrationBuilder.DropColumn(
                name: "isPublished",
                table: "Publications");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PublicationDate",
                table: "Publications",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<string[]>(
                name: "Keywords",
                table: "Publications",
                type: "text[]",
                nullable: false,
                defaultValue: new string[0],
                oldClrType: typeof(string[]),
                oldType: "text[]",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Publications",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CloudStorageGuid",
                table: "Publications",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Abstract",
                table: "Publications",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Department",
                columns: new[] { "DepartmentId", "Name" },
                values: new object[,]
                {
                    { 1, "Кафедра диференціальних рівнянь, геометрії та топології" },
                    { 2, "Кафедра комп'ютерних систем та технологій" },
                    { 3, "Кафедра комп’ютерної алгебри та дискретної математики" },
                    { 4, "Кафедра математичного аналізу" },
                    { 5, "Кафедра математичного забезпечення комп’ютерних систем" },
                    { 6, "Кафедра методів математичної фізики" },
                    { 7, "Кафедра механіки, автоматизації та інформаційних технологій" },
                    { 8, "Кафедра оптимального керування та економічної кібернетики" },
                    { 9, "Кафедра фізики та астрономії" }
                });
        }
    }
}
