using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solidaridad.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class loanBatchItemUpdated2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoanBatchItem_ItemUnit_UnitId",
                table: "LoanBatchItem");

            migrationBuilder.DropForeignKey(
                name: "FK_LoanBatchItem_LoanBatches_LoanBatchId",
                table: "LoanBatchItem");

            migrationBuilder.DropForeignKey(
                name: "FK_LoanBatchItem_LoanItems_LoanItemId",
                table: "LoanBatchItem");

            migrationBuilder.AlterColumn<int>(
                name: "UnitId",
                table: "LoanBatchItem",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_LoanBatchItem_ItemUnit_UnitId",
                table: "LoanBatchItem",
                column: "UnitId",
                principalTable: "ItemUnit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LoanBatchItem_LoanBatches_LoanBatchId",
                table: "LoanBatchItem",
                column: "LoanBatchId",
                principalTable: "LoanBatches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LoanBatchItem_LoanItems_LoanItemId",
                table: "LoanBatchItem",
                column: "LoanItemId",
                principalTable: "LoanItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoanBatchItem_ItemUnit_UnitId",
                table: "LoanBatchItem");

            migrationBuilder.DropForeignKey(
                name: "FK_LoanBatchItem_LoanBatches_LoanBatchId",
                table: "LoanBatchItem");

            migrationBuilder.DropForeignKey(
                name: "FK_LoanBatchItem_LoanItems_LoanItemId",
                table: "LoanBatchItem");

            migrationBuilder.AlterColumn<int>(
                name: "UnitId",
                table: "LoanBatchItem",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

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

            migrationBuilder.AddForeignKey(
                name: "FK_LoanBatchItem_ItemUnit_UnitId",
                table: "LoanBatchItem",
                column: "UnitId",
                principalTable: "ItemUnit",
                principalColumn: "Id");

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
    }
}
