using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solidaridad.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class auditlogissuesfix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "MasterLoanItems",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "MasterLoanItems",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedBy",
                table: "MasterLoanItems",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "MasterLoanItems",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "ItemCategories",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "ItemCategories",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedBy",
                table: "ItemCategories",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "ItemCategories",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Cooperatives",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Cooperatives",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedBy",
                table: "Cooperatives",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "Cooperatives",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "MasterLoanItems");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "MasterLoanItems");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "MasterLoanItems");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "MasterLoanItems");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ItemCategories");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "ItemCategories");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "ItemCategories");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "ItemCategories");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Cooperatives");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Cooperatives");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Cooperatives");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "Cooperatives");
        }
    }
}
