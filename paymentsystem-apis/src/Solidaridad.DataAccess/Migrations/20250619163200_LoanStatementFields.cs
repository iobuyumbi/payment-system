using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solidaridad.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class LoanStatementFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastPaymentDate",
                table: "LoanStatement");

            migrationBuilder.RenameColumn(
                name: "PendingPayments",
                table: "LoanStatement",
                newName: "OpeningBalance");

            migrationBuilder.RenameColumn(
                name: "InterestAccrued",
                table: "LoanStatement",
                newName: "LoanBalance");

            migrationBuilder.RenameColumn(
                name: "CurrentPrincipal",
                table: "LoanStatement",
                newName: "DebitAmount");

            migrationBuilder.AddColumn<decimal>(
                name: "CreditAmount",
                table: "LoanStatement",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "LoanStatement",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TransactionReference",
                table: "LoanStatement",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TransactionType",
                table: "LoanStatement",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreditAmount",
                table: "LoanStatement");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "LoanStatement");

            migrationBuilder.DropColumn(
                name: "TransactionReference",
                table: "LoanStatement");

            migrationBuilder.DropColumn(
                name: "TransactionType",
                table: "LoanStatement");

            migrationBuilder.RenameColumn(
                name: "OpeningBalance",
                table: "LoanStatement",
                newName: "PendingPayments");

            migrationBuilder.RenameColumn(
                name: "LoanBalance",
                table: "LoanStatement",
                newName: "InterestAccrued");

            migrationBuilder.RenameColumn(
                name: "DebitAmount",
                table: "LoanStatement",
                newName: "CurrentPrincipal");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastPaymentDate",
                table: "LoanStatement",
                type: "timestamp with time zone",
                nullable: true);
        }
    }
}
