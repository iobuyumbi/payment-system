using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solidaridad.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class disbursements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentBatch_Countries_CountryId",
                table: "PaymentBatch");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentBatch_LoanBatches_LoanBatchId",
                table: "PaymentBatch");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PaymentBatch",
                table: "PaymentBatch");

            migrationBuilder.RenameTable(
                name: "PaymentBatch",
                newName: "PaymentBatches");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentBatch_LoanBatchId",
                table: "PaymentBatches",
                newName: "IX_PaymentBatches_LoanBatchId");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentBatch_CountryId",
                table: "PaymentBatches",
                newName: "IX_PaymentBatches_CountryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PaymentBatches",
                table: "PaymentBatches",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AssociateMaps",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FarmerId = table.Column<Guid>(type: "uuid", nullable: false),
                    PaymentBatchId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssociateMaps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Disbursements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FarmerId = table.Column<Guid>(type: "uuid", nullable: false),
                    MethodId = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    CurrencyCode = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StatusId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Disbursements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Disbursements_Farmers_FarmerId",
                        column: x => x.FarmerId,
                        principalTable: "Farmers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Disbursements_FarmerId",
                table: "Disbursements",
                column: "FarmerId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentBatches_Countries_CountryId",
                table: "PaymentBatches",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentBatches_LoanBatches_LoanBatchId",
                table: "PaymentBatches",
                column: "LoanBatchId",
                principalTable: "LoanBatches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentBatches_Countries_CountryId",
                table: "PaymentBatches");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentBatches_LoanBatches_LoanBatchId",
                table: "PaymentBatches");

            migrationBuilder.DropTable(
                name: "AssociateMaps");

            migrationBuilder.DropTable(
                name: "Disbursements");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PaymentBatches",
                table: "PaymentBatches");

            migrationBuilder.RenameTable(
                name: "PaymentBatches",
                newName: "PaymentBatch");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentBatches_LoanBatchId",
                table: "PaymentBatch",
                newName: "IX_PaymentBatch_LoanBatchId");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentBatches_CountryId",
                table: "PaymentBatch",
                newName: "IX_PaymentBatch_CountryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PaymentBatch",
                table: "PaymentBatch",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentBatch_Countries_CountryId",
                table: "PaymentBatch",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentBatch_LoanBatches_LoanBatchId",
                table: "PaymentBatch",
                column: "LoanBatchId",
                principalTable: "LoanBatches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
