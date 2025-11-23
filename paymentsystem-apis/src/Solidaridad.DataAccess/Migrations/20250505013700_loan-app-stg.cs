using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solidaridad.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class loanappstg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
