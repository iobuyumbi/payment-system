using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solidaridad.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class loanappstg2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "ExcelImportId",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "RowNumber",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "ValidationErrors",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "ValidationStatus",
                table: "LoanApplications");

            migrationBuilder.AddColumn<Guid>(
                name: "LoanApplicationImportStagingId",
                table: "LoanItems",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LoanApplicationImportStaging",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FarmerId = table.Column<Guid>(type: "uuid", nullable: false),
                    WitnessFullName = table.Column<string>(type: "text", nullable: true),
                    WitnessNationalId = table.Column<string>(type: "text", nullable: true),
                    WitnessPhoneNo = table.Column<string>(type: "text", nullable: true),
                    WitnessRelation = table.Column<string>(type: "text", nullable: true),
                    DateOfWitness = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EnumeratorFullName = table.Column<string>(type: "text", nullable: true),
                    LoanBatchId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PrincipalAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    EffectivePrincipal = table.Column<decimal>(type: "numeric", nullable: false),
                    AccruedInterest = table.Column<decimal>(type: "numeric", nullable: false),
                    InterestAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    InterestRate = table.Column<decimal>(type: "numeric", nullable: false),
                    LoanNumber = table.Column<string>(type: "text", nullable: true),
                    RemainingBalance = table.Column<decimal>(type: "numeric", nullable: false),
                    FeeApplied = table.Column<decimal>(type: "numeric", nullable: false),
                    ExcelImportId = table.Column<Guid>(type: "uuid", nullable: false),
                    RowNumber = table.Column<int>(type: "integer", nullable: false),
                    ValidationStatus = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ValidationErrors = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanApplicationImportStaging", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoanApplicationImportStaging_Farmers_FarmerId",
                        column: x => x.FarmerId,
                        principalTable: "Farmers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LoanItems_LoanApplicationImportStagingId",
                table: "LoanItems",
                column: "LoanApplicationImportStagingId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanApplicationImportStaging_FarmerId",
                table: "LoanApplicationImportStaging",
                column: "FarmerId");

            migrationBuilder.AddForeignKey(
                name: "FK_LoanItems_LoanApplicationImportStaging_LoanApplicationImpor~",
                table: "LoanItems",
                column: "LoanApplicationImportStagingId",
                principalTable: "LoanApplicationImportStaging",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoanItems_LoanApplicationImportStaging_LoanApplicationImpor~",
                table: "LoanItems");

            migrationBuilder.DropTable(
                name: "LoanApplicationImportStaging");

            migrationBuilder.DropIndex(
                name: "IX_LoanItems_LoanApplicationImportStagingId",
                table: "LoanItems");

            migrationBuilder.DropColumn(
                name: "LoanApplicationImportStagingId",
                table: "LoanItems");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "LoanApplications",
                type: "character varying(34)",
                maxLength: 34,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "ExcelImportId",
                table: "LoanApplications",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RowNumber",
                table: "LoanApplications",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ValidationErrors",
                table: "LoanApplications",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ValidationStatus",
                table: "LoanApplications",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
