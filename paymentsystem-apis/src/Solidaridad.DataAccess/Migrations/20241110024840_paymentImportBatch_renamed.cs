using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solidaridad.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class paymentImportBatch_renamed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentImportBatches");

            migrationBuilder.CreateTable(
                name: "PaymentImportSummary",
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
                    table.PrimaryKey("PK_PaymentImportSummary", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentImportSummary");

            migrationBuilder.CreateTable(
                name: "PaymentImportBatches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ExcelImportId = table.Column<Guid>(type: "uuid", nullable: false),
                    FailedRows = table.Column<int>(type: "integer", nullable: false),
                    PassedRows = table.Column<int>(type: "integer", nullable: false),
                    TotalRowsInExcel = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentImportBatches", x => x.Id);
                });
        }
    }
}
