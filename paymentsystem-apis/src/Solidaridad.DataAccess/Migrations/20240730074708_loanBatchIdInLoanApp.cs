using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solidaridad.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class loanBatchIdInLoanApp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LoanBatchId",
                table: "LoanApplications",
                type: "uuid",
                nullable: true);

            

            
           
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Countries_CountryId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CountryId",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_\"LogEntry\"",
                table: "\"LogEntry\"");

            migrationBuilder.DropColumn(
                name: "LoanBatchId",
                table: "LoanApplications");

            migrationBuilder.RenameTable(
                name: "\"LogEntry\"",
                newName: "LogEntry");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LogEntry",
                table: "LogEntry",
                column: "Id");
        }
    }
}
