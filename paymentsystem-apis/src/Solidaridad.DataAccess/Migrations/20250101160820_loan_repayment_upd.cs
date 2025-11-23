using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solidaridad.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class loan_repayment_upd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_LoanRepayment_LoanApplicationId",
                table: "LoanRepayment",
                column: "LoanApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_LoanRepayment_LoanApplications_LoanApplicationId",
                table: "LoanRepayment",
                column: "LoanApplicationId",
                principalTable: "LoanApplications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoanRepayment_LoanApplications_LoanApplicationId",
                table: "LoanRepayment");

            migrationBuilder.DropIndex(
                name: "IX_LoanRepayment_LoanApplicationId",
                table: "LoanRepayment");
        }
    }
}
