using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solidaridad.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class paymentDeduct_updated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PurchasePriceSum",
                table: "PaymentRequestDeductible",
                newName: "UnitCostEur");

            migrationBuilder.RenameColumn(
                name: "PricePerCru",
                table: "PaymentRequestDeductible",
                newName: "TotalUnitsEarningLc");

            migrationBuilder.RenameColumn(
                name: "NetDisbursementAmount",
                table: "PaymentRequestDeductible",
                newName: "TotalUnitsEarningEur");

            migrationBuilder.RenameColumn(
                name: "LoanAdjustmentAmount",
                table: "PaymentRequestDeductible",
                newName: "SolidaridadEarningsShare");

            migrationBuilder.RenameColumn(
                name: "FarmerShareUsd",
                table: "PaymentRequestDeductible",
                newName: "FarmerPayableEarningsLc");

            migrationBuilder.RenameColumn(
                name: "FarmerShareConverted",
                table: "PaymentRequestDeductible",
                newName: "FarmerLoansDeductionsLc");

            migrationBuilder.RenameColumn(
                name: "CruCount",
                table: "PaymentRequestDeductible",
                newName: "FarmerLoansBalanceLc");

            migrationBuilder.RenameColumn(
                name: "ConvertedPurchasePrice",
                table: "PaymentRequestDeductible",
                newName: "FarmerEarningsShareLc");

            migrationBuilder.RenameColumn(
                name: "AdminDeduction",
                table: "PaymentRequestDeductible",
                newName: "FarmerEarningsShareEur");

            migrationBuilder.AddColumn<string>(
                name: "BeneficiaryId",
                table: "PaymentRequestDeductible",
                type: "character varying(126)",
                maxLength: 126,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "CarbonUnitsAccured",
                table: "PaymentRequestDeductible",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "ExcelImportId",
                table: "PaymentRequestDeductible",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "PaymentRequestDeductible",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PaymentImportBatches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ExcelImportId = table.Column<Guid>(type: "uuid", nullable: false),
                    TotalRowsInExcel = table.Column<int>(type: "integer", nullable: false),
                    PassedRows = table.Column<int>(type: "integer", nullable: false),
                    FailedRows = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentImportBatches", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentImportBatches");

            migrationBuilder.DropColumn(
                name: "BeneficiaryId",
                table: "PaymentRequestDeductible");

            migrationBuilder.DropColumn(
                name: "CarbonUnitsAccured",
                table: "PaymentRequestDeductible");

            migrationBuilder.DropColumn(
                name: "ExcelImportId",
                table: "PaymentRequestDeductible");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "PaymentRequestDeductible");

            migrationBuilder.RenameColumn(
                name: "UnitCostEur",
                table: "PaymentRequestDeductible",
                newName: "PurchasePriceSum");

            migrationBuilder.RenameColumn(
                name: "TotalUnitsEarningLc",
                table: "PaymentRequestDeductible",
                newName: "PricePerCru");

            migrationBuilder.RenameColumn(
                name: "TotalUnitsEarningEur",
                table: "PaymentRequestDeductible",
                newName: "NetDisbursementAmount");

            migrationBuilder.RenameColumn(
                name: "SolidaridadEarningsShare",
                table: "PaymentRequestDeductible",
                newName: "LoanAdjustmentAmount");

            migrationBuilder.RenameColumn(
                name: "FarmerPayableEarningsLc",
                table: "PaymentRequestDeductible",
                newName: "FarmerShareUsd");

            migrationBuilder.RenameColumn(
                name: "FarmerLoansDeductionsLc",
                table: "PaymentRequestDeductible",
                newName: "FarmerShareConverted");

            migrationBuilder.RenameColumn(
                name: "FarmerLoansBalanceLc",
                table: "PaymentRequestDeductible",
                newName: "CruCount");

            migrationBuilder.RenameColumn(
                name: "FarmerEarningsShareLc",
                table: "PaymentRequestDeductible",
                newName: "ConvertedPurchasePrice");

            migrationBuilder.RenameColumn(
                name: "FarmerEarningsShareEur",
                table: "PaymentRequestDeductible",
                newName: "AdminDeduction");
        }
    }
}
