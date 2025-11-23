using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solidaridad.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class processFeeUpd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LoanBatchProcessingFee",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FeeName = table.Column<string>(type: "text", nullable: true),
                    FeeType = table.Column<string>(type: "text", nullable: true),
                    Amount = table.Column<decimal>(type: "numeric", nullable: true),
                    Percent = table.Column<decimal>(type: "numeric", nullable: true),
                    LoanBatchId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanBatchProcessingFee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoanBatchProcessingFee_LoanBatches_LoanBatchId",
                        column: x => x.LoanBatchId,
                        principalTable: "LoanBatches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LoanBatchProcessingFee_LoanBatchId",
                table: "LoanBatchProcessingFee",
                column: "LoanBatchId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoanBatchProcessingFee");
        }
    }
}
