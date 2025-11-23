using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solidaridad.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class paymnet_stages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PaymentBatchId",
                table: "PaymentBatchHistory",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_PaymentBatches_StatusId",
                table: "PaymentBatches",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentBatches_MasterPaymentApprovalStage_StatusId",
                table: "PaymentBatches",
                column: "StatusId",
                principalTable: "MasterPaymentApprovalStage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentBatches_MasterPaymentApprovalStage_StatusId",
                table: "PaymentBatches");

            migrationBuilder.DropIndex(
                name: "IX_PaymentBatches_StatusId",
                table: "PaymentBatches");

            migrationBuilder.DropColumn(
                name: "PaymentBatchId",
                table: "PaymentBatchHistory");
        }
    }
}
