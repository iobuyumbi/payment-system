using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solidaridad.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class DisbursementDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropIndex(
            //    name: "IX_Farmers_Mobile",
            //    table: "Farmers");

            //migrationBuilder.DropIndex(
            //    name: "IX_Farmers_PaymentPhoneNumber",
            //    table: "Farmers");

            migrationBuilder.AddColumn<DateTime>(
                name: "DisbursementDate",
                table: "LoanApplications",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisbursementDate",
                table: "LoanApplications");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Farmers_Mobile",
            //    table: "Farmers",
            //    column: "Mobile",
            //    unique: true,
            //    filter: "\"IsDeleted\" = false AND \"Mobile\" IS NOT NULL AND \"Mobile\" <> ''");

            migrationBuilder.CreateIndex(
                name: "IX_Farmers_PaymentPhoneNumber",
                table: "Farmers",
                column: "PaymentPhoneNumber",
                unique: true,
                filter: "\"IsDeleted\" = false AND \"PaymentPhoneNumber\" IS NOT NULL AND \"PaymentPhoneNumber\" <> ''");
        }
    }
}
