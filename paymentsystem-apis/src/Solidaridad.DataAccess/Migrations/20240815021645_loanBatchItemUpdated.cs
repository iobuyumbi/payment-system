using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solidaridad.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class loanBatchItemUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Unit",
                table: "LoanBatchItem");

            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                table: "LoanBatchItem",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LoanBatchItem_UnitId",
                table: "LoanBatchItem",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_LoanBatchItem_ItemUnit_UnitId",
                table: "LoanBatchItem",
                column: "UnitId",
                principalTable: "ItemUnit",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoanBatchItem_ItemUnit_UnitId",
                table: "LoanBatchItem");

            migrationBuilder.DropIndex(
                name: "IX_LoanBatchItem_UnitId",
                table: "LoanBatchItem");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "LoanBatchItem");

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "LoanBatchItem",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
