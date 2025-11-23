using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solidaridad.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class fieldsinstatement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AccuredInterest",
                table: "LoanStatement",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "AccuredPrincipalPayment",
                table: "LoanStatement",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "InterestPaid",
                table: "LoanStatement",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PrincipalPaid",
                table: "LoanStatement",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccuredInterest",
                table: "LoanStatement");

            migrationBuilder.DropColumn(
                name: "AccuredPrincipalPayment",
                table: "LoanStatement");

            migrationBuilder.DropColumn(
                name: "InterestPaid",
                table: "LoanStatement");

            migrationBuilder.DropColumn(
                name: "PrincipalPaid",
                table: "LoanStatement");
        }
    }
}
