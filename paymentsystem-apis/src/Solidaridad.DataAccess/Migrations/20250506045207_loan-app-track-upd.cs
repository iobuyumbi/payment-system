using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solidaridad.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class loanapptrackupd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "LoanApplications");

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "LoanApplicationImportStaging",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "StatusId",
                table: "LoanApplicationImportStaging",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "LoanApplicationImportStaging");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "LoanApplicationImportStaging");

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "LoanApplications",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "StatusId",
                table: "LoanApplications",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);
        }
    }
}
