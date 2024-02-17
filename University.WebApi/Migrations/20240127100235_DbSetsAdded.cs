using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace University.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class DbSetsAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HeadOfDepartment_AspNetUsers_ApplicationUserId",
                table: "HeadOfDepartment");

            migrationBuilder.DropForeignKey(
                name: "FK_HeadOfDepartment_Department_DepartmentId",
                table: "HeadOfDepartment");

            migrationBuilder.DropForeignKey(
                name: "FK_Lecturers_Department_DepartmentId",
                table: "Lecturers");

            migrationBuilder.DropForeignKey(
                name: "FK_Plan_Department_DepartmentId",
                table: "Plan");

            migrationBuilder.DropForeignKey(
                name: "FK_Publications_Plan_PlanId",
                table: "Publications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Plan",
                table: "Plan");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HeadOfDepartment",
                table: "HeadOfDepartment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Department",
                table: "Department");

            migrationBuilder.RenameTable(
                name: "Plan",
                newName: "Plans");

            migrationBuilder.RenameTable(
                name: "HeadOfDepartment",
                newName: "HeadsOfDepartments");

            migrationBuilder.RenameTable(
                name: "Department",
                newName: "Departments");

            migrationBuilder.RenameIndex(
                name: "IX_Plan_DepartmentId",
                table: "Plans",
                newName: "IX_Plans_DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_HeadOfDepartment_DepartmentId",
                table: "HeadsOfDepartments",
                newName: "IX_HeadsOfDepartments_DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_HeadOfDepartment_ApplicationUserId",
                table: "HeadsOfDepartments",
                newName: "IX_HeadsOfDepartments_ApplicationUserId");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "HeadsOfDepartments",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Plans",
                table: "Plans",
                column: "PlanId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HeadsOfDepartments",
                table: "HeadsOfDepartments",
                column: "HeadOfDepartmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Departments",
                table: "Departments",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_HeadsOfDepartments_AspNetUsers_ApplicationUserId",
                table: "HeadsOfDepartments",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HeadsOfDepartments_Departments_DepartmentId",
                table: "HeadsOfDepartments",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lecturers_Departments_DepartmentId",
                table: "Lecturers",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Plans_Departments_DepartmentId",
                table: "Plans",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_HeadsOfDepartments_AspNetUsers_ApplicationUserId",
                table: "HeadsOfDepartments");

            migrationBuilder.DropForeignKey(
                name: "FK_HeadsOfDepartments_Departments_DepartmentId",
                table: "HeadsOfDepartments");

            migrationBuilder.DropForeignKey(
                name: "FK_Lecturers_Departments_DepartmentId",
                table: "Lecturers");

            migrationBuilder.DropForeignKey(
                name: "FK_Plans_Departments_DepartmentId",
                table: "Plans");

            migrationBuilder.DropForeignKey(
                name: "FK_Publications_Plans_PlanId",
                table: "Publications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Plans",
                table: "Plans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HeadsOfDepartments",
                table: "HeadsOfDepartments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Departments",
                table: "Departments");

            migrationBuilder.RenameTable(
                name: "Plans",
                newName: "Plan");

            migrationBuilder.RenameTable(
                name: "HeadsOfDepartments",
                newName: "HeadOfDepartment");

            migrationBuilder.RenameTable(
                name: "Departments",
                newName: "Department");

            migrationBuilder.RenameIndex(
                name: "IX_Plans_DepartmentId",
                table: "Plan",
                newName: "IX_Plan_DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_HeadsOfDepartments_DepartmentId",
                table: "HeadOfDepartment",
                newName: "IX_HeadOfDepartment_DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_HeadsOfDepartments_ApplicationUserId",
                table: "HeadOfDepartment",
                newName: "IX_HeadOfDepartment_ApplicationUserId");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "HeadOfDepartment",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Plan",
                table: "Plan",
                column: "PlanId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HeadOfDepartment",
                table: "HeadOfDepartment",
                column: "HeadOfDepartmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Department",
                table: "Department",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_HeadOfDepartment_AspNetUsers_ApplicationUserId",
                table: "HeadOfDepartment",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HeadOfDepartment_Department_DepartmentId",
                table: "HeadOfDepartment",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lecturers_Department_DepartmentId",
                table: "Lecturers",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Plan_Department_DepartmentId",
                table: "Plan",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Publications_Plan_PlanId",
                table: "Publications",
                column: "PlanId",
                principalTable: "Plan",
                principalColumn: "PlanId",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
