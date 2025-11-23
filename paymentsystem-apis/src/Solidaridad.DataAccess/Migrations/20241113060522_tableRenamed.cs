using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solidaridad.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class tableRenamed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoanProcessingFees_MasterLoanTerm_MasterLoanTermId",
                table: "LoanProcessingFees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LoanProcessingFees",
                table: "LoanProcessingFees");

            migrationBuilder.RenameTable(
                name: "LoanProcessingFees",
                newName: "MasterLoanTermAdditionalFee");

            migrationBuilder.RenameIndex(
                name: "IX_LoanProcessingFees_MasterLoanTermId",
                table: "MasterLoanTermAdditionalFee",
                newName: "IX_MasterLoanTermAdditionalFee_MasterLoanTermId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MasterLoanTermAdditionalFee",
                table: "MasterLoanTermAdditionalFee",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MasterLoanTermAdditionalFee_MasterLoanTerm_MasterLoanTermId",
                table: "MasterLoanTermAdditionalFee",
                column: "MasterLoanTermId",
                principalTable: "MasterLoanTerm",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MasterLoanTermAdditionalFee_MasterLoanTerm_MasterLoanTermId",
                table: "MasterLoanTermAdditionalFee");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MasterLoanTermAdditionalFee",
                table: "MasterLoanTermAdditionalFee");

            migrationBuilder.RenameTable(
                name: "MasterLoanTermAdditionalFee",
                newName: "LoanProcessingFees");

            migrationBuilder.RenameIndex(
                name: "IX_MasterLoanTermAdditionalFee_MasterLoanTermId",
                table: "LoanProcessingFees",
                newName: "IX_LoanProcessingFees_MasterLoanTermId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LoanProcessingFees",
                table: "LoanProcessingFees",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LoanProcessingFees_MasterLoanTerm_MasterLoanTermId",
                table: "LoanProcessingFees",
                column: "MasterLoanTermId",
                principalTable: "MasterLoanTerm",
                principalColumn: "Id");
        }
    }
}
