using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solidaridad.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class coopIdinLoanApp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CooperativeId",
                table: "LoanApplications",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_LogEntry",
                table: "LogEntry");

            migrationBuilder.DropColumn(
                name: "CooperativeId",
                table: "LoanApplications");

            migrationBuilder.RenameTable(
                name: "LogEntry",
                newName: "\"LogEntry\"");

            migrationBuilder.AddPrimaryKey(
                name: "PK_\"LogEntry\"",
                table: "\"LogEntry\"",
                column: "Id");
        }
    }
}
