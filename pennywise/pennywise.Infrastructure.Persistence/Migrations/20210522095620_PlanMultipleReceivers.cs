using Microsoft.EntityFrameworkCore.Migrations;

namespace pennywise.Infrastructure.Persistence.Migrations
{
    public partial class PlanMultipleReceivers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PaymentPlanId",
                table: "BankDetails",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BankDetails_PaymentPlanId",
                table: "BankDetails",
                column: "PaymentPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_BankDetails_PaymentPlans_PaymentPlanId",
                table: "BankDetails",
                column: "PaymentPlanId",
                principalTable: "PaymentPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankDetails_PaymentPlans_PaymentPlanId",
                table: "BankDetails");

            migrationBuilder.DropIndex(
                name: "IX_BankDetails_PaymentPlanId",
                table: "BankDetails");

            migrationBuilder.DropColumn(
                name: "PaymentPlanId",
                table: "BankDetails");
        }
    }
}
