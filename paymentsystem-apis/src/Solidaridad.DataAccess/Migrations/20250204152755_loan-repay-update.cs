using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solidaridad.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class loanrepayupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoanBalance",
                table: "LoanRepayment");

            migrationBuilder.DropColumn(
                name: "LoanRepaid",
                table: "LoanRepayment");

            migrationBuilder.DropColumn(
                name: "NewPrinciaplAmount",
                table: "LoanRepayment");

            migrationBuilder.RenameColumn(
                name: "LoanAmount",
                table: "LoanRepayment",
                newName: "AmountPaid");

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDate",
                table: "LoanRepayment",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "PaymentMode",
                table: "LoanRepayment",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferenceNumber",
                table: "LoanRepayment",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentDate",
                table: "LoanRepayment");

            migrationBuilder.DropColumn(
                name: "PaymentMode",
                table: "LoanRepayment");

            migrationBuilder.DropColumn(
                name: "ReferenceNumber",
                table: "LoanRepayment");

            migrationBuilder.RenameColumn(
                name: "AmountPaid",
                table: "LoanRepayment",
                newName: "LoanAmount");

            migrationBuilder.AddColumn<decimal>(
                name: "LoanBalance",
                table: "LoanRepayment",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "LoanRepaid",
                table: "LoanRepayment",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "NewPrinciaplAmount",
                table: "LoanRepayment",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);
        }
    }
}
