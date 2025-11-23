using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solidaridad.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class loanrepaupdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InterestRate",
                table: "LoanInterest");

            migrationBuilder.DropColumn(
                name: "LoanStartDate",
                table: "LoanInterest");

            migrationBuilder.DropColumn(
                name: "Principal",
                table: "LoanInterest");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "InterestRate",
                table: "LoanInterest",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "LoanStartDate",
                table: "LoanInterest",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "Principal",
                table: "LoanInterest",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }
    }
}
