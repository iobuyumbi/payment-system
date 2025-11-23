using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solidaridad.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class loanBatchUpdated2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "LoanItemId",
                table: "LoanBatchItem",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "LoanBatchId",
                table: "LoanBatchItem",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateIndex(
                name: "IX_LoanBatchItem_LoanBatchId",
                table: "LoanBatchItem",
                column: "LoanBatchId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanBatchItem_LoanItemId",
                table: "LoanBatchItem",
                column: "LoanItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_LoanBatchItem_LoanBatches_LoanBatchId",
                table: "LoanBatchItem",
                column: "LoanBatchId",
                principalTable: "LoanBatches",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LoanBatchItem_LoanItems_LoanItemId",
                table: "LoanBatchItem",
                column: "LoanItemId",
                principalTable: "LoanItems",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoanBatchItem_LoanBatches_LoanBatchId",
                table: "LoanBatchItem");

            migrationBuilder.DropForeignKey(
                name: "FK_LoanBatchItem_LoanItems_LoanItemId",
                table: "LoanBatchItem");

            migrationBuilder.DropIndex(
                name: "IX_LoanBatchItem_LoanBatchId",
                table: "LoanBatchItem");

            migrationBuilder.DropIndex(
                name: "IX_LoanBatchItem_LoanItemId",
                table: "LoanBatchItem");

            migrationBuilder.AlterColumn<Guid>(
                name: "LoanItemId",
                table: "LoanBatchItem",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "LoanBatchId",
                table: "LoanBatchItem",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);
        }
    }
}
