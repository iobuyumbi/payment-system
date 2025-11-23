using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solidaridad.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class loanapptrackupd_new : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoanApplicationImportStaging_Farmers_FarmerId",
                table: "LoanApplicationImportStaging");

            migrationBuilder.DropForeignKey(
                name: "FK_LoanItems_LoanApplicationImportStaging_LoanApplicationImpor~",
                table: "LoanItems");

            migrationBuilder.DropIndex(
                name: "IX_LoanItems_LoanApplicationImportStagingId",
                table: "LoanItems");

            migrationBuilder.DropIndex(
                name: "IX_LoanApplicationImportStaging_FarmerId",
                table: "LoanApplicationImportStaging");

            migrationBuilder.DropColumn(
                name: "LoanApplicationImportStagingId",
                table: "LoanItems");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "LoanApplicationImportStaging");

            migrationBuilder.AlterColumn<Guid>(
                name: "FarmerId",
                table: "LoanApplicationImportStaging",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LoanApplicationImportStagingId",
                table: "LoanItems",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "FarmerId",
                table: "LoanApplicationImportStaging",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "LoanApplicationImportStaging",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LoanItems_LoanApplicationImportStagingId",
                table: "LoanItems",
                column: "LoanApplicationImportStagingId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanApplicationImportStaging_FarmerId",
                table: "LoanApplicationImportStaging",
                column: "FarmerId");

            migrationBuilder.AddForeignKey(
                name: "FK_LoanApplicationImportStaging_Farmers_FarmerId",
                table: "LoanApplicationImportStaging",
                column: "FarmerId",
                principalTable: "Farmers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LoanItems_LoanApplicationImportStaging_LoanApplicationImpor~",
                table: "LoanItems",
                column: "LoanApplicationImportStagingId",
                principalTable: "LoanApplicationImportStaging",
                principalColumn: "Id");
        }
    }
}
