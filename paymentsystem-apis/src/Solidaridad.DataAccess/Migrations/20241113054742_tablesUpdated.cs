using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solidaridad.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class tablesUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "LoanProcessingFees");

            migrationBuilder.DropColumn(
                name: "FlatAmount",
                table: "LoanProcessingFees");

            migrationBuilder.DropColumn(
                name: "Percentage",
                table: "LoanProcessingFees");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "LoanProcessingFees");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "LoanBatchProcessingFee");

            migrationBuilder.DropColumn(
                name: "Percent",
                table: "LoanBatchProcessingFee");

            migrationBuilder.DropColumn(
                name: "ProcessingFee",
                table: "LoanBatches");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "LoanProcessingFees",
                newName: "MasterLoanTermId");

            migrationBuilder.RenameColumn(
                name: "IsPercentageBased",
                table: "LoanProcessingFees",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "LoanProcessingFees",
                newName: "LoanBatchId");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Wallets",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TodoLists",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TodoItems",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Projects",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Permission",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PaymentRequestFacilitation",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PaymentRequestDeductible",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PaymentImportSummary",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PaymentBatches",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PaymenBatchLoanBatchMapping",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Modules",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "MasterLoanItems",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "FeeName",
                table: "LoanProcessingFees",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FeeType",
                table: "LoanProcessingFees",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Value",
                table: "LoanProcessingFees",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "LoanItems",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "LoanCategories",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "Value",
                table: "LoanBatchProcessingFee",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "LoanBatchItem",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "LoanBatches",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "LoanApplications",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ItemCategories",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Farmers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "FarmerCooperative",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ExcelImportDetail",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ExcelImport",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "EmailTemplateVariable",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Disbursements",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Countries",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Cooperatives",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Constituencies",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AttachmentMappings",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AttachmentFile",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AssociateMaps",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ApplicationStatusLogs",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ApplicationStatus",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AdminLevel4",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AdminLevel3",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AdminLevel2",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AdminLevel1",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Addresses",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "MasterLoanTerm",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DescriptiveName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    InterestRateType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    InterestRate = table.Column<decimal>(type: "numeric", nullable: false),
                    InterestApplication = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Tenure = table.Column<int>(type: "integer", nullable: false),
                    GracePeriod = table.Column<int>(type: "integer", nullable: false),
                    HasAdditionalFee = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterLoanTerm", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LoanProcessingFees_MasterLoanTermId",
                table: "LoanProcessingFees",
                column: "MasterLoanTermId");

            migrationBuilder.AddForeignKey(
                name: "FK_LoanProcessingFees_MasterLoanTerm_MasterLoanTermId",
                table: "LoanProcessingFees",
                column: "MasterLoanTermId",
                principalTable: "MasterLoanTerm",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoanProcessingFees_MasterLoanTerm_MasterLoanTermId",
                table: "LoanProcessingFees");

            migrationBuilder.DropTable(
                name: "MasterLoanTerm");

            migrationBuilder.DropIndex(
                name: "IX_LoanProcessingFees_MasterLoanTermId",
                table: "LoanProcessingFees");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Wallets");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TodoLists");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TodoItems");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Permission");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "PaymentRequestFacilitation");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "PaymentRequestDeductible");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "PaymentImportSummary");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "PaymentBatches");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "PaymenBatchLoanBatchMapping");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "MasterLoanItems");

            migrationBuilder.DropColumn(
                name: "FeeType",
                table: "LoanProcessingFees");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "LoanProcessingFees");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "LoanItems");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "LoanCategories");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "LoanBatchProcessingFee");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "LoanBatchItem");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "LoanBatches");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ItemCategories");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Farmers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "FarmerCooperative");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ExcelImportDetail");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ExcelImport");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "EmailTemplateVariable");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Disbursements");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Cooperatives");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Constituencies");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AttachmentMappings");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AttachmentFile");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AssociateMaps");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ApplicationStatusLogs");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ApplicationStatus");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AdminLevel4");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AdminLevel3");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AdminLevel2");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AdminLevel1");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Addresses");

            migrationBuilder.RenameColumn(
                name: "MasterLoanTermId",
                table: "LoanProcessingFees",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "LoanBatchId",
                table: "LoanProcessingFees",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "LoanProcessingFees",
                newName: "IsPercentageBased");

            migrationBuilder.AlterColumn<string>(
                name: "FeeName",
                table: "LoanProcessingFees",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "LoanProcessingFees",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "FlatAmount",
                table: "LoanProcessingFees",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Percentage",
                table: "LoanProcessingFees",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "LoanProcessingFees",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "LoanBatchProcessingFee",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Percent",
                table: "LoanBatchProcessingFee",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ProcessingFee",
                table: "LoanBatches",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
