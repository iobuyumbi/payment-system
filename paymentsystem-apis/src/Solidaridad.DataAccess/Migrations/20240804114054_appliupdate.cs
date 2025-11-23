using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solidaridad.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class appliupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "LoanApplications");

            migrationBuilder.AddColumn<string>(
                name: "unit",
                table: "LoanItems",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LostReason",
                table: "LoanApplications",
                type: "text",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminLevel1",
                table: "LoanApplications",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AdminLevel2",
                table: "LoanApplications",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AdminLevel3",
                table: "LoanApplications",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AdminLevel4",
                table: "LoanApplications",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "CountryId",
                table: "LoanApplications",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "unit",
                table: "LoanItems");

            migrationBuilder.DropColumn(
                name: "AdminLevel1",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "AdminLevel2",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "AdminLevel3",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "AdminLevel4",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "LoanApplications");

            migrationBuilder.AlterColumn<Guid>(
                name: "LostReason",
                table: "LoanApplications",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "LoanApplications",
                type: "text",
                nullable: true);
        }
    }
}
