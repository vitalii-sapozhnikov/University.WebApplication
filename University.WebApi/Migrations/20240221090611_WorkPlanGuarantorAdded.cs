using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace University.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class WorkPlanGuarantorAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GuarantorId",
                table: "WorkPlans",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_WorkPlans_GuarantorId",
                table: "WorkPlans",
                column: "GuarantorId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkPlans_AspNetUsers_GuarantorId",
                table: "WorkPlans",
                column: "GuarantorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkPlans_AspNetUsers_GuarantorId",
                table: "WorkPlans");

            migrationBuilder.DropIndex(
                name: "IX_WorkPlans_GuarantorId",
                table: "WorkPlans");

            migrationBuilder.DropColumn(
                name: "GuarantorId",
                table: "WorkPlans");
        }
    }
}
