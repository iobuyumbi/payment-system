using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solidaridad.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class paymentBatchLoanBatchMap : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentBatches_LoanBatches_LoanBatchId",
                table: "PaymentBatches");

            migrationBuilder.DropIndex(
                name: "IX_PaymentBatches_LoanBatchId",
                table: "PaymentBatches");

            migrationBuilder.DropColumn(
                name: "LoanBatchId",
                table: "PaymentBatches");

            migrationBuilder.DropColumn(
                name: "PaymentBatchId",
                table: "ExcelImportDetail");

            migrationBuilder.AddColumn<Guid>(
                name: "PaymentBatchId",
                table: "PaymentRequestDeductible",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "PaymenBatchLoanBatchMapping",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PaymentBatchId = table.Column<Guid>(type: "uuid", nullable: false),
                    LoanBatchId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymenBatchLoanBatchMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymenBatchLoanBatchMapping_PaymentBatches_PaymentBatchId",
                        column: x => x.PaymentBatchId,
                        principalTable: "PaymentBatches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymenBatchLoanBatchMapping_PaymentBatchId",
                table: "PaymenBatchLoanBatchMapping",
                column: "PaymentBatchId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymenBatchLoanBatchMapping");

            migrationBuilder.DropColumn(
                name: "PaymentBatchId",
                table: "PaymentRequestDeductible");

            migrationBuilder.AddColumn<Guid>(
                name: "LoanBatchId",
                table: "PaymentBatches",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "PaymentBatchId",
                table: "ExcelImportDetail",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_PaymentBatches_LoanBatchId",
                table: "PaymentBatches",
                column: "LoanBatchId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentBatches_LoanBatches_LoanBatchId",
                table: "PaymentBatches",
                column: "LoanBatchId",
                principalTable: "LoanBatches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
