using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solidaridad.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class colomncorr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LoanbatchId",
                table: "PaymentBatch",
                newName: "LoanBatchId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentBatch_CountryId",
                table: "PaymentBatch",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentBatch_LoanBatchId",
                table: "PaymentBatch",
                column: "LoanBatchId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentBatch_Countries_CountryId",
                table: "PaymentBatch",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentBatch_LoanBatches_LoanBatchId",
                table: "PaymentBatch",
                column: "LoanBatchId",
                principalTable: "LoanBatches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentBatch_Countries_CountryId",
                table: "PaymentBatch");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentBatch_LoanBatches_LoanBatchId",
                table: "PaymentBatch");

            migrationBuilder.DropIndex(
                name: "IX_PaymentBatch_CountryId",
                table: "PaymentBatch");

            migrationBuilder.DropIndex(
                name: "IX_PaymentBatch_LoanBatchId",
                table: "PaymentBatch");

            migrationBuilder.RenameColumn(
                name: "LoanBatchId",
                table: "PaymentBatch",
                newName: "LoanbatchId");
        }
    }
}
