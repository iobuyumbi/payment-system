using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solidaridad.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class correction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentModule",
                table: "LoanBatches");

            migrationBuilder.AddColumn<int>(
                name: "PaymentModule",
                table: "PaymentBatches",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentModule",
                table: "PaymentBatches");

            migrationBuilder.AddColumn<int>(
                name: "PaymentModule",
                table: "LoanBatches",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
